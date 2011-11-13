using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;

namespace TauberMatching.Services
{
    public class EmailJob:IJob
    {
        public void Execute(JobExecutionContext context)
        {            
            EmailQueueService.SendMailsInTheQueue();
        }
    }
}