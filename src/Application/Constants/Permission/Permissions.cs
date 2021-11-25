// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CleanArchitecture.Razor.Application.Constants.Permission
{
    public static class Permissions
    {
        [DisplayName("Журналы аудита")]
        [Description("AuditTrails Permissions")]
        public static class AuditTrails
        {
            public const string View = "Permissions.AuditTrails.View";
            public const string Search = "Permissions.AuditTrails.Search";
            public const string Export = "Permissions.AuditTrails.Export";
        }
        [DisplayName("Логи")]
        [Description("Logs Permissions")]
        public static class Logs
        {
            public const string View = "Permissions.Logs.View";
            public const string Search = "Permissions.Logs.Search";
            public const string Export = "Permissions.Logs.Export";
        }
        //[DisplayName("Workflow")]
        //[Description("Approval Permissions")]
        //public static class Approval
        //{
        //    public const string View = "Permissions.Approval.View";
        //    public const string Approve = "Permissions.Approval.Approve";
        //    public const string Histories = "Permissions.Approval.Histories";
        //    public const string Search = "Permissions.Approval.Search";
        //    public const string Export = "Permissions.Approval.Export";
        //}

        //[DisplayName("Products")]
        //[Description("Products Permissions")]
        //public static class Products
        //{
        //    public const string View = "Permissions.Products.View";
        //    public const string Create = "Permissions.Products.Create";
        //    public const string Edit = "Permissions.Products.Edit";
        //    public const string Delete = "Permissions.Products.Delete";
        //    public const string Search = "Permissions.Products.Search";
        //    public const string Export = "Permissions.Products.Export";
        //    public const string Import = "Permissions.Products.Import";
        //}
        //[DisplayName("Customers")]
        //[Description("Customers Permissions")]
        //public static class Customers
        //{
        //    public const string View = "Permissions.Customers.View";
        //    public const string Create = "Permissions.Customers.Create";
        //    public const string Edit = "Permissions.Customers.Edit";
        //    public const string Delete = "Permissions.Customers.Delete";
        //    public const string Search = "Permissions.Customers.Search";
        //    public const string Export = "Permissions.Customers.Export";
        //    public const string Import = "Permissions.Customers.Import";
        //}



        //[DisplayName("Documents")]
        //[Description("Documents Permissions")]
        //public static class Documents
        //{
        //    public const string View = "Permissions.Documents.View";
        //    public const string Create = "Permissions.Documents.Create";
        //    public const string Edit = "Permissions.Documents.Edit";
        //    public const string Delete = "Permissions.Documents.Delete";
        //    public const string Search = "Permissions.Documents.Search";
        //    public const string Export = "Permissions.Documents.Export";
        //    public const string Import = "Permissions.Documents.Import";
        //    public const string Download = "Permissions.Documents.Download";
        //}
        //[DisplayName("DocumentTypes")]
        //[Description("DocumentTypes Permissions")]
        //public static class DocumentTypes
        //{
        //    public const string View = "Permissions.DocumentTypes.View";
        //    public const string Create = "Permissions.DocumentTypes.Create";
        //    public const string Edit = "Permissions.DocumentTypes.Edit";
        //    public const string Delete = "Permissions.DocumentTypes.Delete";
        //    public const string Search = "Permissions.DocumentTypes.Search";
        //    public const string Export = "Permissions.Documents.Export";
        //    public const string Import = "Permissions.Categories.Import";
        //}
        //[DisplayName("Dictionaries")]
        //[Description("Dictionaries Permissions")]
        //public static class Dictionaries
        //{
        //    public const string View = "Permissions.Dictionaries.View";
        //    public const string Create = "Permissions.Dictionaries.Create";
        //    public const string Edit = "Permissions.Dictionaries.Edit";
        //    public const string Delete = "Permissions.Dictionaries.Delete";
        //    public const string Search = "Permissions.Dictionaries.Search";
        //    public const string Export = "Permissions.Dictionaries.Export";
        //    public const string Import = "Permissions.Dictionaries.Import";
        //}

        [DisplayName("Пользователи")]
        [Description("Users Permissions")]
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
            public const string Search = "Permissions.Users.Search";
            
            public const string Export = "Permissions.Dictionaries.Export";
            public const string ManageRoles = "Permissions.Users.ManageRoles";
            public const string RestPassword = "Permissions.Users.RestPassword";
            public const string Active = "Permissions.Users.Active";
        }

        [DisplayName("Роли")]
        [Description("Roles Permissions")]
        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            public const string Search = "Permissions.Roles.Search";
            public const string Export = "Permissions.Roles.Export";
            
            public const string ManagePermissions = "Permissions.Roles.Permissions";
            public const string ManageNavigation = "Permissions.Roles.Navigation";
        }

        [DisplayName("Требования к Роли")]
        [Description("Role Claims Permissions")]
        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
            public const string Search = "Permissions.RoleClaims.Search";
        }



        [DisplayName("Dashboards")]
        [Description("Dashboards Permissions")]
        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
        }

        [DisplayName("Hangfire")]
        [Description("Hangfire Permissions")]
        public static class Hangfire
        {
            public const string View = "Permissions.Hangfire.View";
        }

        //Karavay
        [DisplayName("Направления")]
        [Description("Directions Permissions")]
        public static class Directions
        {
            public const string View = "Permissions.Directions.View";
            public const string Create = "Permissions.Directions.Create";
            public const string Edit = "Permissions.Directions.Edit";
            public const string Delete = "Permissions.Directions.Delete";
            public const string Search = "Permissions.Directions.Search";
            public const string Export = "Permissions.Directions.Export";
         
        }
        [DisplayName("Категория")]
        [Description("Categories Permissions")]
        public static class Categories
        {
            public const string View = "Permissions.Categories.View";
            public const string Create = "Permissions.Categories.Create";
            public const string Edit = "Permissions.Categories.Edit";
            public const string Delete = "Permissions.Categories.Delete";
            public const string Search = "Permissions.Categories.Search";
            public const string Export = "Permissions.Categories.Export";
            
        }
        [DisplayName("Поставщики")]
        [Description("Contragents Permissions")]
        public static class Contragents
        {
            public const string View = "Permissions.Contragents.View";
            public const string Create = "Permissions.Contragents.Create";
            public const string Edit = "Permissions.Contragents.Edit";
            public const string Delete = "Permissions.Contragents.Delete";
            public const string DeleteFile = "Permissions.Contragents.DeleteFile";
            public const string Search = "Permissions.Contragents.Search";
            public const string Export = "Permissions.Contragents.Export";
            
            public const string Acceditation = "Permissions.Contragents.Acceditation";
        }
        [DisplayName("Статус лог")]
        [Description("StatusLogs Permissions")]
        public static class StatusLogs
        {
            public const string View = "Permissions.StatusLogs.View";

        }

        [DisplayName("Ед. измерения")]
        [Description("UnitOfs Permissions")]
        public static class UnitOfs
        {
            public const string View = "Permissions.UnitOfs.View";
            public const string Create = "Permissions.UnitOfs.Create";
            public const string Edit = "Permissions.UnitOfs.Edit";
            public const string Delete = "Permissions.UnitOfs.Delete";
            public const string Search = "Permissions.UnitOfs.Search";
            public const string Export = "Permissions.UnitOfs.Export";
            
        }

        [DisplayName("НДС")]
        [Description("Vats Permissions")]
        public static class Vats
        {
            public const string View = "Permissions.Vats.View";
            public const string Create = "Permissions.Vats.Create";
            public const string Edit = "Permissions.Vats.Edit";
            public const string Delete = "Permissions.Vats.Delete";
            public const string Search = "Permissions.Vats.Search";
            public const string Export = "Permissions.Vats.Export";
            
        }
        [DisplayName("Производственные площадки")]
        [Description("Areas Permissions")]
        public static class Areas
        {
            public const string View = "Permissions.Areas.View";
            public const string Create = "Permissions.Areas.Create";
            public const string Edit = "Permissions.Areas.Edit";
            public const string Delete = "Permissions.Areas.Delete";
            public const string Search = "Permissions.Areas.Search";
            public const string Export = "Permissions.Areas.Export";
            
        }
        [DisplayName("Документы по качеству")]
        [Description("QualityDocs Permissions")]
        public static class QualityDocs
        {
            public const string View = "Permissions.QualityDocs.View";
            public const string Create = "Permissions.QualityDocs.Create";
            public const string Edit = "Permissions.QualityDocs.Edit";
            public const string Delete = "Permissions.QualityDocs.Delete";
            public const string Search = "Permissions.QualityDocs.Search";
            public const string Export = "Permissions.QualityDocs.Export";
            
        }
        //Nomenclatures
        [DisplayName("Номенклатура")]
        [Description("Nomenclatures Permissions")]
        public static class Nomenclatures
        {
            public const string View = "Permissions.Nomenclatures.View";
            public const string Create = "Permissions.Nomenclatures.Create";
            public const string Edit = "Permissions.Nomenclatures.Edit";
            public const string Delete = "Permissions.Nomenclatures.Delete";
            public const string DeleteFile = "Permissions.Nomenclatures.DeleteFile";
            public const string Search = "Permissions.Nomenclatures.Search";
            public const string Export = "Permissions.Nomenclatures.Export";
            
        }

        //ComOffers
        [DisplayName("Коммерческое предложения")]
        [Description("Offers Permissions")]
        public static class ComOffers
        {
            public const string View = "Permissions.ComOffers.View";
            public const string Create = "Permissions.ComOffers.Create";
            public const string Edit = "Permissions.ComOffers.Edit";
            public const string Delete = "Permissions.ComOffers.Delete";
            public const string DeleteFile = "Permissions.ComOffers.DeleteFile";
            public const string Search = "Permissions.ComOffers.Search";
            public const string Export = "Permissions.ComOffers.Export";

        }
        /// <summary>
        /// Returns a list of Permissions.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRegisteredPermissions()
        {
            var permissions = new List<string>();
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    permissions.Add(propertyValue.ToString());
            }
            return permissions;
        }


    }
}
