﻿using System;
using System.Threading.Tasks;

using Quartz;

namespace Wyn.Quartz.Abstractions
{
    public abstract class TaskAbstract : ITask
    {
        protected readonly ITaskLogger Logger;

        protected TaskAbstract(ITaskLogger logger)
        {
            Logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobId = context.JobDetail.JobDataMap["id"];
            Logger.JobId = jobId == null ? Guid.Empty : Guid.Parse(jobId.ToString());

            await Logger.Info("任务开始");

            try
            {
                await Execute(new TaskExecutionContext
                {
                    JobId = Logger.JobId,
                    JobExecutionContext = context
                });
            }
            catch (Exception ex)
            {
                await Logger.Error("任务异常：" + ex);
            }

            await Logger.Info("任务结束");
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract Task Execute(ITaskExecutionContext context);
    }
}
