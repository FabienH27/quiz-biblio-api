using Hangfire;
using QuizBiblio.JobScheduler.Jobs;

namespace QuizBiblio.JobScheduler;

public static class JobsInitializer
{
    public static void StartJobs()
    {
        //Cleans temporary images on Cloud Storage every day
        RecurringJob.AddOrUpdate<StorageCleanupJob>("storage-cleanup", job => job.PerformCleanup(), Cron.Daily);
    }
}
