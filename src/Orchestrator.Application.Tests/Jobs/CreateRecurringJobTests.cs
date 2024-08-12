using Hangfire.JobsLogger;
using Hangfire.Server;
using Moq;
using Orchestrator.Application;
using Orchestrator.Domain.Entities;
using SharedUtilLibrary;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
namespace Orchestrator.Application.Tests;

public class CreateRecurringScheduleJobTests
{
    [Fact]
    public async Task ExecuteJobAsync_Should_ExecuteJobAndAddExecutedEvent()
    {
        // Arrange
        var scheduleId = "scheduleId";
        var schedule = new Domain.Entities.Schedule
        {
            ScheduleId = scheduleId,
            Name = "Test Schedule",
            CronExpression = "*/5 * * * *", // Set the CronExpression
            Topic = "Test Topic", // Set the Topic
            ProcessCode = "Test ProcessCode" // Set the ProcessCode
        };

        var scheduleRepositoryMock = new Mock<IMongoRepository<Domain.Entities.Schedule>>();
        scheduleRepositoryMock.Setup(x => x.FindOne(It.IsAny<Expression<Func<Domain.Entities.Schedule, bool>>>()))
            .ReturnsAsync(schedule);

        var executedEventRepositoryMock = new Mock<IMongoRepository<ExecutedEvent>>();

        var performContextMock = new Mock<PerformContext>();

        var job = new CreateRecurringScheduleJob(scheduleRepositoryMock.Object, executedEventRepositoryMock.Object);

        // Act
        await job.ExecuteJobAsync(performContextMock.Object, scheduleId);

        // Assert
        scheduleRepositoryMock.Verify(x => x.FindOne(It.IsAny<Expression<Func<Domain.Entities.Schedule, bool>>>()), Times.Once);
        executedEventRepositoryMock.Verify(x => x.InsertOne(It.IsAny<ExecutedEvent>()), Times.Once);
        performContextMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.Exactly(3));
        performContextMock.Verify(x => x.LogWarning(It.IsAny<string>()), Times.Never);
        performContextMock.Verify(x => x.LogError(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteJobAsync_Should_LogWarning_WhenScheduleNotFound()
    {
        // Arrange
        var scheduleId = "scheduleId";

        var scheduleRepositoryMock = new Mock<IMongoRepository<Domain.Entities.Schedule>>();
        scheduleRepositoryMock.Setup(x => x.FindOne(It.IsAny<Expression<Func<Domain.Entities.Schedule, bool>>>()))
            .ReturnsAsync((Domain.Entities.Schedule)null);

        var executedEventRepositoryMock = new Mock<IMongoRepository<ExecutedEvent>>();

        var performContextMock = new Mock<PerformContext>();

        var job = new CreateRecurringScheduleJob(scheduleRepositoryMock.Object, executedEventRepositoryMock.Object);

        // Act
        await job.ExecuteJobAsync(performContextMock.Object, scheduleId);

        // Assert
        scheduleRepositoryMock.Verify(x => x.FindOne(It.IsAny<Expression<Func<Domain.Entities.Schedule, bool>>>()), Times.Once);
        executedEventRepositoryMock.Verify(x => x.InsertOne(It.IsAny<ExecutedEvent>()), Times.Never);
        performContextMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.Never);
        performContextMock.Verify(x => x.LogWarning(It.IsAny<string>()), Times.Once);
        performContextMock.Verify(x => x.LogError(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteJobAsync_Should_LogError_WhenExceptionThrown()
    {
        // Arrange
        var scheduleId = "scheduleId";
        var exceptionMessage = "Test Exception";

        var scheduleRepositoryMock = new Mock<IMongoRepository<Domain.Entities.Schedule>>();
        scheduleRepositoryMock.Setup(x => x.FindOne(It.IsAny<Expression<Func<Domain.Entities.Schedule, bool>>>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        var executedEventRepositoryMock = new Mock<IMongoRepository<ExecutedEvent>>();

        var performContextMock = new Mock<PerformContext>();

        var job = new CreateRecurringScheduleJob(scheduleRepositoryMock.Object, executedEventRepositoryMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => job.ExecuteJobAsync(performContextMock.Object, scheduleId));

        scheduleRepositoryMock.Verify(x => x.FindOne(It.IsAny<Expression<Func<Domain.Entities.Schedule, bool>>>()), Times.Once);
        executedEventRepositoryMock.Verify(x => x.InsertOne(It.IsAny<ExecutedEvent>()), Times.Never);
        performContextMock.Verify(x => x.LogInformation(It.IsAny<string>()), Times.Never);
        performContextMock.Verify(x => x.LogWarning(It.IsAny<string>()), Times.Never);
        performContextMock.Verify(x => x.LogError(It.IsAny<string>()), Times.Once);
        performContextMock.Verify(x => x.LogError($"Error in job {scheduleId}: {exceptionMessage}"), Times.Once);
    }
}