using System;

using Quartz;

namespace Wyn.Quartz.Abstractions
{
    public class TaskExecutionContext : ITaskExecutionContext
    {
        public Guid JobId { get; set; }

        public IJobExecutionContext JobExecutionContext { get; set; }
    }
}
