using Hangfire;
using Hangfire.Server;
using Hangfire.JobsLogger;
using Hangfire.Dashboard.Management.Metadata;
using Hangfire.Dashboard.Management.Support;
using System.ComponentModel;
using SharedUtilLibrary;
using Orchestrator.Domain.Entities;

namespace Orchestrator.Application;

[ManagementPage("Jobs")]
public class CreateRecurringScheduleJob(IMongoRepository<Domain.Entities.Schedule> scheduleRepository, IMongoRepository<ExecutedEvent> executedEventRepository)
{

    [Job]
    [DisplayName(nameof(ExecuteJobAsync))]
    [Description("Execute a job")]
    [AutomaticRetry(Attempts = 0)] 
    public async Task ExecuteJobAsync(PerformContext context, string scheduleId)
    {
        try
        {
            context.LogInformation($"Starting job {scheduleId}");

            var schedule = await scheduleRepository.FindOne(x => x.ScheduleId == scheduleId);
            if (schedule == null)
            {
                context.LogWarning($"Job {scheduleId} not found");
                return;
            }
            context.LogInformation($"Found job {scheduleId} with name {schedule.Name}");

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

            //TODO: Implement job execution logic here, e.g.: publish event to event grid, added message queue

            context.LogInformation($"Finished job {scheduleId}");
        }
        catch (Exception ex)
        {
            context.LogError($"Error in job {scheduleId}: {ex.Message}");
            throw;
        }
    }
}
