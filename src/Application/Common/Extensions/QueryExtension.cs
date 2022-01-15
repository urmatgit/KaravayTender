// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Common.Extensions
{
   public static class QueryExtension
    {
        private static string GetColumnName(this MemberInfo info)
        {
            List<ColumnAttribute> list = info.GetCustomAttributes<ColumnAttribute>().ToList();
            return list.Count > 0 ? list.Single().Name : info.Name;
        }
        /// <summary>
        /// Executes raw query with parameters and maps returned values to column property names of Model provided.
        /// Not all properties are required to be present in model (if not present - null)
        /// </summary>
        public static async IAsyncEnumerable<T> ExecuteQuery<T>(
            [NotNull] this DbContext db,
            [NotNull] string query,
                        [NotNull] params SqlParameter[] parameters)
            where T : class, new()
        {
            await using DbCommand command = db.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            await db.Database.OpenConnectionAsync();
            await using DbDataReader reader = await command.ExecuteReaderAsync();
            List<PropertyInfo> lstColumns = new T().GetType()
                .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
            while (await reader.ReadAsync())
            {
                T newObject = new();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string name = reader.GetName(i);
                    PropertyInfo prop = lstColumns.FirstOrDefault(a => a.GetColumnName().Equals(name));
                    if (prop == null)
                    {
                        continue;
                    }
                    object val = await reader.IsDBNullAsync(i) ? null : reader[i];
                    prop.SetValue(newObject, val, null);
                }
                yield return newObject;
            }
        }

        /// <summary>
        /// Execute raw SQL query with query parameters
        /// </summary>
        /// <typeparam name="T">the return type</typeparam>
        /// <param name="db">the database context database, usually _context.Database</param>
        /// <param name="query">the query string</param>
        /// <param name="map">the map to map the result to the object of type T</param>
        /// <param name="queryParameters">the collection of query parameters, if any</param>
        /// <returns></returns>
        public static List<T> ExecuteSqlRawExt<T, P>(this DbContext db, string query, Func<DbDataReader, T> map, IEnumerable<P> queryParameters = null)
        {
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                if ((queryParameters?.Any() ?? false))
                    command.Parameters.AddRange(queryParameters.ToArray());

                command.CommandText = query;
                command.CommandType = CommandType.Text;

                db.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (result.Read())
                    {
                        entities.Add(map(result));
                    }

                    return entities;
                }
            }

        }
        public static IQueryable<ParticipantCrossDto> GetParicipantsForTab(this IApplicationDbContext _context, int comOfferId)
        {
            var dataStep1 = from c in _context.ComStages
                            join p in _context.StageParticipants on c.Id equals p.ComStageId
                            where c.ComOfferId == comOfferId
                            group c by new { c.ComOfferId, p.ContragentId } into gr
                            select new
                            {
                                ComOfferId = gr.Key.ComOfferId,
                                ContragentId = gr.Key.ContragentId,

                                Number = gr.Max(m => m.Number)

                            };
            var dataStep2 = from a in dataStep1
                            join c in _context.ComStages on new { Id = a.ComOfferId, Number = a.Number } equals new { Id = c.ComOfferId, Number = c.Number }
                            select new
                            {
                                a,
                                ComStageID = c.Id
                            };
            var dataStep3 = from a in dataStep2
                            join b in _context.StageParticipants on new { Id = a.ComStageID, ContrId = a.a.ContragentId } equals new { Id = b.ComStageId, ContrId = b.ContragentId }

                            select new ParticipantCrossDto
                            {
                                ComOfferId = a.a.ComOfferId,
                                ContragentId =b.ContragentId,
                                Number = a.a.Number,
                                ComStageId = a.ComStageID,
                                Status = b.Status,
                                Description = b.Description
                            };
            return dataStep3;
        }
    }

}
