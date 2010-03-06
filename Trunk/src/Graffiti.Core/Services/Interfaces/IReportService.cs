using System;
using System.Collections.Generic;

namespace Graffiti.Core.Services 
{
    public interface IReportService 
    {
        IDictionary<DateTime, int> ViewsByDate(DateTime min, DateTime max, Delegate getInclauseForReadPermissionsCallback);
        ReportData ViewsByDateSingle(DateTime date, Delegate getInclauseForReadPermissionsCallback);
        ReportData ViewsByPost(DateTime min, DateTime max, Delegate getInclauseForReadPermissionsCallback);
        IDictionary<DateTime, int> ViewsByPostSingle(int postId, DateTime min, DateTime max);
        int ViewsByPostSingleCount(int postId, DateTime min, DateTime max);
        ReportData CommentsByDateSingle(DateTime date, Delegate getInclauseForReadPermissionsCallback);
        ReportData CommentsByPost(DateTime min, DateTime max, Delegate getInclauseForReadPermissionsCallback);
        IDictionary<DateTime, int> CommentsByPostSingle(int postId, DateTime min, DateTime max);
        int CommentsByPostSingleCount(int postId, DateTime min, DateTime max);
        IDictionary<DateTime, int> CommentsByDate(DateTime min, DateTime max, Delegate getInclauseForReadPermissionsCallback);
        ReportData MostPopularPosts(Delegate getInclauseForReadPermissionsCallback);
    }
}
