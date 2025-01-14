﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JiraRestClient.Net.Domain.Issue
{
    public class Issue : KeyBase
    {
        [JsonPropertyName("expand")]
        public string Expand { get; set; }

        //[JsonPropertyName("key")]
        //public string Key { get; set; }

        [JsonPropertyName("fields")]
        public IssueFields Fields { get; set; }

        [JsonPropertyName("renderedFields")]
        public RenderedFields RenderedFields { get; set; }
        
        [JsonPropertyName("transitions")]
        public List<Transition> Transitions { get; set; }

        [JsonPropertyName("changelog")]
        public ChangeLog Changelog { get; set; }
    }
    
    public class Aggregateprogress
    {
        [JsonPropertyName("progress")]
        public int? Progress { get; set; }
        [JsonPropertyName("total")]
        public int? Total { get; set; }
    }
    
    public class Attachment
    {
        [JsonPropertyName("self")]
        public string Self { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("filename")]
        public string Filename { get; set; }
        [JsonPropertyName("author")]
        public User Author { get; set; }
        [JsonPropertyName("created")]
        public string Created { get; set; }
        [JsonPropertyName("size")]
        public int? Size { get; set; }
        [JsonPropertyName("mimetype")]
        public string MimeType { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }
    }
    
    public class Attrs
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
        
        [JsonPropertyName("level")]
        public int Level { get; set; }
    }
    
    public class Content
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("content")]
        public List<Content> Contents { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("attrs")]
        public Attrs Attrs { get; set; }
    }
    
    public class Description
    {
        [JsonPropertyName("version")]
        public int Version { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("content")]
        public List<Content> Content { get; set; }
    }


    public class IssueFields
    {
        [JsonPropertyName("parent")]
        public Issue Parent { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }
        [JsonPropertyName("progress")]
        public Progress Progress { get; set; }

        [JsonPropertyName("timetracking")]
        public Timetracking Timetracking { get; set; }

        [JsonPropertyName("issuetype")]
        public IssueType Issuetype { get; set; }
        [JsonPropertyName("votes")]
        public Votes Votes { get; set; }
        [JsonPropertyName("resolution")]
        public Resolution Resolution { get; set; }
        [JsonPropertyName("fixversions")]
        public List<Version> FixVersions { get; set; }
        [JsonPropertyName("resolutiondate")]
        public string Resolutiondate { get; set; }
        [JsonPropertyName("timespent")]
        public int? Timespent { get; set; }
        [JsonPropertyName("creator")]
        public User Creator { get; set; }
        [JsonPropertyName("reporter")]
        public User Reporter { get; set; }
        [JsonPropertyName("aggregatetimeoriginalestimate")]
        public int? Aggregatetimeoriginalestimate { get; set; }
        [JsonPropertyName("created")]
        public string Created { get; set; }
        [JsonPropertyName("updated")]
        public string Updated { get; set; }
        [JsonPropertyName("description")]
        public Description Description { get; set; }
        [JsonPropertyName("priority")]
        public Priority Priority { get; set; }
        [JsonPropertyName("duedate")]
        public string Duedate { get; set; }
        [JsonPropertyName("issuelinks")]
        public List<object> Issuelinks { get; set; }
        [JsonPropertyName("watches")]
        public Watches Watches { get; set; }
        [JsonPropertyName("worklog")]
        public Worklog Worklog { get; set; }
        [JsonPropertyName("subtasks")]
        public List<object> Subtasks { get; set; }
        [JsonPropertyName("status")]
        public Status Status { get; set; }
        [JsonPropertyName("labels")]
        public List<string> Labels { get; set; }
        [JsonPropertyName("workratio")]
        public int? Workratio { get; set; }
        [JsonPropertyName("assignee")]
        public User Assignee { get; set; }
        [JsonPropertyName("attachment")]
        public List<Attachment> Attachment { get; set; }
        [JsonPropertyName("aggregatetimeestimate")]
        public int? Aggregatetimeestimate { get; set; }
        [JsonPropertyName("project")]
        public Project Project { get; set; }
        [JsonPropertyName("versions")]
        public List<Version> Versions { get; set; }
        [JsonPropertyName("environment")]
        public string Environment { get; set; }
        [JsonPropertyName("timestimate")]
        public int? Timeestimate { get; set; }
        [JsonPropertyName("aggregateprogress")]
        public Aggregateprogress Aggregateprogress { get; set; }
        [JsonPropertyName("lastviewed")]
        public string LastViewed { get; set; }
        [JsonPropertyName("components")]
        public List<Component> Components { get; set; }
        [JsonPropertyName("comment")]
        public CommentList CommentList { get; set; }


        int sp = 0;

        [JsonPropertyName("customfield_10031")]
        public float StoryPoint { get; set; }
        //{ 
        //    get { return sp; };
        //    set {

        //    }; 
        //}

        [JsonPropertyName("timeoriginalestimate")]
        public int? Timeoriginalestimate { get; set; }
        [JsonPropertyName("aggregatetimespent")]
        public int? Aggregatetimespent { get; set; }
    }

     //"timetracking": {
     //       "originalEstimate": "4m",
     //       "remainingEstimate": "3m",
     //   }

public class Timetracking
    {
        [JsonPropertyName("originalestimate")]
        public string OriginalEstimate { get; set; }
        [JsonPropertyName("originalestimateseconds")]
        public int? OriginalEstimateSeconds { get; set; }
    }

    public class Votes
    {
        [JsonPropertyName("self")]
        public string Self { get; set; }
        [JsonPropertyName("votes")]
        public int? Vote { get; set; }
        [JsonPropertyName("hasvoted")]
        public bool HasVoted { get; set; }
    }

    public class Progress
    {
        [JsonPropertyName("progress")]
        public int? ProgressValue { get; set; }
        [JsonPropertyName("total")]
        public int? Total { get; set; }
    }

    public class AvatarUrls
    {
        [JsonPropertyName("16x16")] 
        public string Url16X16 { get; set; }
        [JsonPropertyName("24x24")] 
        public string Url24X24 { get; set; }
        [JsonPropertyName("32x32")] 
        public string Url32X32 { get; set; }
        [JsonPropertyName("48x48")] 
        public string Url48X48 { get; set; }
    }
    
    public class Priority : Base
    {
        [JsonPropertyName("iconurl")]
        public string IconUrl { get; set; }
    }

    public class Watches
    {
        [JsonPropertyName("self")]
        public string Self { get; set; }
        [JsonPropertyName("watchcount")]
        public int? WatchCount { get; set; }
        [JsonPropertyName("iswatching")]
        public bool IsWatching { get; set; }
    }

    public class Worklog
    {
        [JsonPropertyName("startat")]
        public int? StartAt { get; set; }
        [JsonPropertyName("maxresults")]
        public int? MaxResults { get; set; }
        [JsonPropertyName("total")]
        public int? Total { get; set; }
        [JsonPropertyName("worklogs")]
        public List<object> Worklogs { get; set; }
    }

    public class StatusCategory : KeyBase
    {
        [JsonPropertyName("colorname")]
        public string ColorName { get; set; }
    }

    public class Status : Base
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("iconurl")]
        public string IconUrl { get; set; }
        [JsonPropertyName("statuscategory")]
        public StatusCategory StatusCategory { get; set; }
        [JsonPropertyName("statusColor")]
        public string StatusColor { get; set; }
    }
    
    public class CommentList
    {
        public int? StartAt { get; set; }
        public int? MaxResults { get; set; }
        public int? Total { get; set; }
        public List<Comment> Comments { get; set; }
    }

    public class Resolution : Base
    {
        public string Description { get; set; }
    }

    public class Comment : Base
    {
        public User Author { get; set; }

        public string Body { get; set; }

        public User UpdateAuthor { get; set; }

        public string Created { get; set; }

        public string Updated { get; set; }
    }

    public class RenderedFields : Base
    {
        [JsonPropertyName("aggregatetimeestimate")]
        public string Aggregatetimeestimate { get; set; }

        [JsonPropertyName("aggregatetimeoriginalestimate")]
        public string Aggregatetimeoriginalestimate { get; set; }

        [JsonPropertyName("timetracking")]
        public Timetracking Timetracking { get; set; }

        [JsonPropertyName("environment")]
        public string Environment { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("updated")]
        public string Updated { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("timeestimate")]
        public string Timeestimate { get; set; }

        [JsonPropertyName("duedate")]
        public string Duedate { get; set; }

        [JsonPropertyName("lastviewed")]
        public string LastViewed { get; set; }

        [JsonPropertyName("attachment")]
        public List<Attachment> Attachment { get; set; }

        [JsonPropertyName("comment")]
        public CommentList Comment { get; set; }

        [JsonPropertyName("timeoriginalestimate")]
        public string Timeoriginalestimate { get; set; }

        [JsonPropertyName("timespent")]
        public object Timespent { get; set; }

        [JsonPropertyName("worklog")]
        public Worklog Worklog { get; set; }

        [JsonPropertyName("aggregatetimespent")]
        public object Aggregatetimespent { get; set; }
    }

    public class ChangeLog : Base
    {
        [JsonPropertyName("startat")]
        public int StartAt { get; set; }

        [JsonPropertyName("maxresults")]
        public int MaxResults { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("histories")]
        public List<History> Histories { get; set; }
    }

    public class History : Base
    {
        [JsonPropertyName("author")]
        public User Author { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }
    }

    public class Item : Base
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }

        [JsonPropertyName("fieldtype")]
        public string Fieldtype { get; set; }

        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("fromstring")]
        public string FromString { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }
    }

    public class Transition : KeyBase
    {
        
        [JsonPropertyName("fields")]
        public TransitionFields Fields { get; set; }
        
        [JsonPropertyName("to")]
        public Status To { get; set; }
    }

    public class TransitionFields
    {
    }
}