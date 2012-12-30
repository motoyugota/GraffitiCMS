using System;
using System.Collections.Generic;
using System.Data;
using Graffiti.Core.Services;

namespace Graffiti.Core
{
    public static class Reports
    {
        private static IRolePermissionService _rolePermissionService = ServiceLocator.Get<IRolePermissionService>();
        private static IReportService _reportService = ServiceLocator.Get<IReportService>();

        public static IDictionary<DateTime, int> ViewsByDate(DateTime min, DateTime max)
        {
            GetInClauseForReadPermissionsCheck callback = (_rolePermissionService.GetInClauseForReadPermissions);
            return _reportService.ViewsByDate(min, max, callback);
        }

        public static ReportData ViewsByDateSingle(DateTime date)
        {
            GetInClauseForReadPermissionsCheck callback = (_rolePermissionService.GetInClauseForReadPermissions);
            return _reportService.ViewsByDateSingle(date, callback);
        }

        public static ReportData ViewsByPost(DateTime min, DateTime max)
        {
            GetInClauseForReadPermissionsCheck callback = (_rolePermissionService.GetInClauseForReadPermissions);
            return _reportService.ViewsByPost(min, max, callback);
        }

        public static IDictionary<DateTime, int> ViewsByPostSingle(int postId, DateTime min, DateTime max)
        {
            return _reportService.ViewsByPostSingle(postId, min, max);
        }

        public static int ViewsByPostSingleCount(int postId, DateTime min, DateTime max)
        {
            return _reportService.ViewsByPostSingleCount(postId, min, max);
        }

        public static ReportData CommentsByDateSingle(DateTime date)
        {
            GetInClauseForReadPermissionsCheck callback = (_rolePermissionService.GetInClauseForReadPermissions);
            return _reportService.CommentsByDateSingle(date, callback);
        }

        public static ReportData CommentsByPost(DateTime min, DateTime max)
        {
            GetInClauseForReadPermissionsCheck callback = (_rolePermissionService.GetInClauseForReadPermissions);
            return _reportService.CommentsByPost(min, max, callback);
        }

        public static IDictionary<DateTime, int> CommentsByPostSingle(int postId, DateTime min, DateTime max)
        {
            return _reportService.CommentsByPostSingle(postId, min, max);
        }

        public static int CommentsByPostSingleCount(int postId, DateTime min, DateTime max)
        {
            return _reportService.CommentsByPostSingleCount(postId, min, max);
        }

        public static IDictionary<DateTime, int> CommentsByDate(DateTime min, DateTime max)
        {
            GetInClauseForReadPermissionsCheck callback = (_rolePermissionService.GetInClauseForReadPermissions);
            return _reportService.CommentsByDate(min, max, callback);
        }

        public static ReportData MostPopularPosts()
        {
            GetInClauseForReadPermissionsCheck callback = (_rolePermissionService.GetInClauseForReadPermissions);
            return _reportService.MostPopularPosts(callback);
        }
    }
}
