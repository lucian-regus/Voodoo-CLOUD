using Domain.Models;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.CompilerServices;
using Quartz;

namespace Infrastructure.Jobs;

public class IpSumJob : IJob
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;
    
    public IpSumJob(IConfiguration configuration, HttpClient httpClient, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var fileCount = IntegerType.FromString(_configuration["IPsum:NumberOfFiles"]);
        
        var fetchBlacklistedIpsTask = _unitOfWork.BlacklistedIpAddressRepository.GetAllAsync(true);
    
        var downloadTasks = Enumerable.Range(1, fileCount)
            .Select(fileNumber => _httpClient.GetStringAsync($"{fileNumber}.txt"))
            .ToList();
        var fileContents = await Task.WhenAll(downloadTasks);

        var scrapedIpAddresses = new HashSet<string>(
            fileContents.SelectMany(content => content.Split('\n'))
        );
        
        var blacklistedIpsFromDb = (await fetchBlacklistedIpsTask)
            .Select(record => record.IpAddress)
            .ToHashSet();

        scrapedIpAddresses.ExceptWith(blacklistedIpsFromDb);

        var newScrapingLog = new ScrapingLog
        {
            Date = DateTime.Now
        };
        
        var newBlacklistedIpEntries = scrapedIpAddresses.Select(ip => new BlacklistedIpAddress
        {
            IpAddress = ip,
            ScrapingLogId = newScrapingLog.Id
        });
        
        _unitOfWork.ScrapingLogRepository.Add(newScrapingLog);
        _unitOfWork.BlacklistedIpAddressRepository.AddRange(newBlacklistedIpEntries);
        await _unitOfWork.SaveChangesAsync();
    }
}