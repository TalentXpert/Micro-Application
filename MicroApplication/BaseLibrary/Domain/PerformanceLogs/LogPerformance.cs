using BaseLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Domain.PerformanceLogs
{

    public class LogPerformanceRecord : IDisposable
    {
        private Stopwatch Timer { get; set; }
        public string Method { get; private set; }
        public long Duration { get; set; }
        private long Elapsed { get { return Timer.ElapsedMilliseconds; } }
        public LogPerformanceRecord(string method)
        {
            Method = method;
            Timer = new Stopwatch();
            Timer.Start();
        }

        public void Dispose()
        {
            Timer.Stop();
            Duration = Elapsed;
        }
    }


    public class LogPerformance : IDisposable
    {
        private string _callingMethodName;
        private string actionId;
        private string _controllerName;
        private Guid? _userId;
        private string _detail;

        List<LogPerformanceRecord> _methods = new List<LogPerformanceRecord>();
        //public LogPerformance(string controller, string method, string actionId, Guid? userId = null) : this()
        //{
        //    Initialize(controller, method, userId);
        //}

        public LogPerformance()
        {
            _callingMethodName = "EntryMethod";
            _controllerName = "Entry";
            actionId = Guid.NewGuid().ToString();
            Timer = new Stopwatch();
            Timer.Start();
        }

        public void Initialize(string controller, string method, Guid? userId = null)
        {
            _callingMethodName = method;
            this._userId = userId;
            _controllerName = controller;
        }

        private long Elapsed { get { return Timer.ElapsedMilliseconds; } }

        private Stopwatch Timer { get; set; }


        public void Dispose()
        {
            Timer.Stop();
            SetDetail();
            WriteLogEntry();
        }
        private void SetDetail()
        {
            if (_methods.Count < 5)
            {
                foreach (var record in _methods)
                    _detail += $"{record.Method}-{record.Duration}{Environment.NewLine}";
            }
            else
            {
                _detail = _methods.Count.ToString();
            }
        }

        public void SetUserId(Guid? userId)
        {
            _userId = userId;
        }

        private void WriteLogEntry()
        {
            var elapsedMilliseconds = Elapsed;
            if (elapsedMilliseconds > 1000)
            {
                var repository = GetRepository();
                repository.Add(new PerformanceLog { Method = _callingMethodName, Duration = (int)elapsedMilliseconds, Controller = _controllerName, ActionId = actionId, ApplicationUserId = _userId, Detail = _detail });
                repository.UnitOfWork.Commit();
            }
        }

        public LogPerformanceRecord MethodExecutionStarted(string method)
        {
            method = GetParentMethod(method);
            var methodRecord = new LogPerformanceRecord(method);
            _methods.Add(methodRecord);
            return methodRecord;
        }

        private string GetParentMethod(string method)
        {
            var parent = _methods.LastOrDefault(p => p.Duration == 0);
            if (parent != null) return parent.Method + "->" + method;
            return method;
        }

        private PerformanceLogRepository GetRepository()
        {
            return new PerformanceLogRepository(GetContext(), NullLoggerFactory.Instance);
        }
        private static DbContextOptions GetDbContextOptions()
        {
            return new DbContextOptionsBuilder().UseSqlServer(ApplicationSettingBase.DatabaseConnectionString).Options;
        }

        private static IBaseDatabase GetContext()
        {
            return new BaseDatabase(GetDbContextOptions(), new DataAnnotationsEntityValidator());
        }
    }
}
