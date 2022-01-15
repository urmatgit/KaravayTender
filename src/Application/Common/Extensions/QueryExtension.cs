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
    }

}
