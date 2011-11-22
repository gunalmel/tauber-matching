using System.Web.Mvc;
using System.Web.Routing;
using Quartz;
using Quartz.Impl;
using Microsoft.Web.Mvc;
using TauberMatching.Services;

namespace TauberMatching
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory()); // Depends on MVC2Futures library to make MVC ver2 controllers to accept JSON serialized objects
            log4net.Config.XmlConfigurator.Configure();
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            //TODO Protect Quartz from getting recycled. #9
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            
            // construct job info
            JobDetail jobDetail = new JobDetail("mySendMailJob", typeof(EmailJob));
            // fire every minute to check for queued e-mail messages in the db
            Trigger trigger = TriggerUtils.MakeMinutelyTrigger(1);
            trigger.Name = "mySendMailTrigger";
            // schedule the job for execution
            sched.ScheduleJob(jobDetail, trigger);
            sched.Start();
            log4net.ILog log = log4net.LogManager.GetLogger(typeof(MvcApplication));
            log.Info("Quartz Queue started! Application Started!");

            //To flush logger buffer immediatley if not configured in the config file
            //foreach (IAppender appender in log.Logger.Repository.GetAppenders())
            //{
            //    var buffered = appender as BufferingAppenderSkeleton;
            //    if (buffered != null)
            //    {
            //        buffered.Flush();
            //    }
            //}
        }
    }
}