using System;
using System.Collections.Generic;
using System.Linq;
using Graffiti.Core;
using Graffiti.Core.Services;
using Graffiti.Data;

namespace Graffiti.Services 
{
    public class ReportService : IReportService
    {
        private IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public IDictionary<System.DateTime, int> ViewsByDate(System.DateTime min, System.DateTime max, Delegate getInclauseForReadPermissionsCallback) 
        {
            return _reportRepository.ViewsByDate(min, max, getInclauseForReadPermissionsCallback);
        }

        public ReportData ViewsByDateSingle(System.DateTime date, Delegate getInclauseForReadPermissionsCallback) 
        {
            return _reportRepository.ViewsByDateSingle(date, getInclauseForReadPermissionsCallback);
        }

        public ReportData ViewsByPost(System.DateTime min, System.DateTime max, Delegate getInclauseForReadPermissionsCallback) 
        {
            return _reportRepository.ViewsByPost(min, max, getInclauseForReadPermissionsCallback);
        }

        public IDictionary<System.DateTime, int> ViewsByPostSingle(int postId, System.DateTime min, System.DateTime max) 
        {
            return ViewsByPostSingle(postId, min, max);
        }

        public int ViewsByPostSingleCount(int postId, System.DateTime min, System.DateTime max) 
        {
            return ViewsByPostSingleCount(postId, min, max);
        }

        public ReportData CommentsByDateSingle(System.DateTime date, Delegate getInclauseForReadPermissionsCallback) 
        {
            return _reportRepository.CommentsByDateSingle(date, getInclauseForReadPermissionsCallback);
        }

        public ReportData CommentsByPost(System.DateTime min, System.DateTime max, Delegate getInclauseForReadPermissionsCallback) 
        {
            return _reportRepository.CommentsByPost(min, max, getInclauseForReadPermissionsCallback);
        }

        public IDictionary<System.DateTime, int> CommentsByPostSingle(int postId, System.DateTime min, System.DateTime max) 
        {
            return _reportRepository.CommentsByPostSingle(postId, min, max);
        }

        public int CommentsByPostSingleCount(int postId, System.DateTime min, System.DateTime max) 
        {
            return _reportRepository.CommentsByPostSingleCount(postId, min, max);
        }

        public IDictionary<System.DateTime, int> CommentsByDate(System.DateTime min, System.DateTime max, Delegate getInclauseForReadPermissionsCallback) 
        {
            return _reportRepository.CommentsByDate(min, max, getInclauseForReadPermissionsCallback);
        }

        public ReportData MostPopularPosts(Delegate getInclauseForReadPermissionsCallback) 
        {
            return _reportRepository.MostPopularPosts(getInclauseForReadPermissionsCallback);
        }

    }
}
