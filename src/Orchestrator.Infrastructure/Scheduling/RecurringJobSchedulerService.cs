using Hangfire.Server;
using Orchestrator.Core.Interfaces;
using SharedUtilLibrary.Interfaces.TaskManager;
using Hangfire;
using Hangfire.JobsLogger;
using Hangfire.Dashboard.Management.Support;
using System.ComponentModel;
using SharedUtilLibrary;
using Orchestrator.Domain.Entities;
using Microsoft.Extensions.Logging;


namespace Orchestrator.Infrastructure;

public class RecurringJobSchedulerService(ILogger<RecurringJobSchedulerService> logger, IJobScheduler jobScheduler, IMongoRepository<Schedule> scheduleRepository, IMongoRepository<ExecutedEvent> executedEventRepository) : IRecurringJobScheduler
{    
    public string CreateRecurringScheduleJob(string ScheduleId, string name, string cronExpression)
    {
       return  jobScheduler.ScheduleJob<RecurringJobSchedulerService>(x => x.ExecuteJobAsync(default, ScheduleId), name, cronExpression);
    }

    
    [Job]
    [DisplayName(nameof(ExecuteJobAsync))]
    [Description("Execute a job")]
    [AutomaticRetry(Attempts = 0)] 
    public async Task ExecuteJobAsync(PerformContext context, string scheduleId)
    {
        try
        {
            context.LogInformation($"Starting job {scheduleId}");
            logger.LogInformation($"Starting job {scheduleId}",scheduleId);

            var schedule = await scheduleRepository.FindOne(x => x.ScheduleId == scheduleId);
            if (schedule == null)
            {
                context.LogWarning($"Job {scheduleId} not found");
                logger.LogWarning($"Job {scheduleId} not found", scheduleId);
                return;
            }
            context.LogInformation($"Found job {scheduleId} with name {schedule.Name}");
            logger.LogInformation($"Found job {scheduleId} with name {schedule.Name}",schedule.Name);

            var executedEvent = new ExecutedEvent
            {
                ScheduleId = schedule.ScheduleId,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                ExecutionTime = DateTime.UtcNow,
                LastExecuteDate = DateTime.UtcNow,
                Status = "Success"
            };

            await executedEventRepository.InsertOne(executedEvent);

            context.LogInformation($"Added executed event for job {scheduleId}");
            logger.LogInformation($"Added executed event for job {scheduleId}",scheduleId);


            //TODO: Implement job execution logic here, e.g.: publish event to event grid, added message queue

            context.LogInformation($"Finished job {scheduleId}");
            logger.LogInformation($"Finished job {scheduleId}", scheduleId);
        }
        catch (Exception ex)
        {
            context.LogError($"Error in job {scheduleId}: {ex.Message}");
            logger.LogError($"Error in job {scheduleId}: {ex.Message}", ex.Message);
            throw;
        }
    }
}
