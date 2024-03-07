using System;

namespace JiraRestClient.Net.Test;

public abstract class BaseTest
{
    protected readonly Uri Uri = new("https://unidataplatform.atlassian.net/");

    /// <summary>
    /// To get a User from the RestApi
    /// </summary>
    protected const string AccountId = "712020:33dde142-de22-42ed-9b80-1fe1b22a57a5";

    protected const string Username = "anibal.yeh@uni-psg.com";

    protected const string Password = "ATATT3xFfGF0gJRxjdv_O4PmXtQ4lrFCad_vVBJ8xGjhsZqcLKEULEicuvz4Y_fZ-RLj50Z4gRiHnLFiolxy3oxnhyiTWVDtvOF7WuZaaS_kuh1wa1TJn8xS5yKE-hKMZ43muRs0NPhkc6Dk05e8LKwuBKjUUEMX3KcADmYma697R83KZI-qbMw=B998632F";

    protected const string ProjectKey = "ST";

    protected const string IssueKeyToSearch = "ST-1";

    protected readonly JiraRestClient RestClient;
    
    protected Uri BaseUri;

    public BaseTest(){
        RestClient =  new JiraRestClient(Uri, Username, Password);
        RestClient.Login(Username, Password);
        BaseUri = RestClient.BaseUri;
    }
}