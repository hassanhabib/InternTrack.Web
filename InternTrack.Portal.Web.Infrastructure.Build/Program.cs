// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Collections.Generic;
using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

namespace InternTrack.Portal.Web.Infrastructure.Build
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var client = new ADotNetClient();

            var gitHubPipeline = new GithubPipeline
            {
                Name = "Intern Portal Build",

                OnEvents = new Events
                {
                    Push = new PushEvent
                    {
                        Branches = new string[] { "master" }
                    },

                    PullRequest = new PullRequestEvent
                    {
                        Branches = new string[] { "master" }
                    }
                },

                Jobs = new Jobs
                {
                    Build = new BuildJob
                    {
                        RunsOn = BuildMachines.WindowsLatest,

                        Steps = new List<GithubTask>
                        {
                            new CheckoutTaskV2
                            {
                                Name = "Checking Out Code"
                            },

                            new SetupDotNetTaskV1
                            {
                                Name = "Installing .NET",

                                TargetDotNetVersion = new TargetDotNetVersion
                                {
                                    DotNetVersion = "7.0.306"
                                }
                            },

                            new RestoreTask
                            {
                                Name = "Restoring Nuget Packages"
                            },

                            new DotNetBuildTask
                            {
                                Name = "Building Project"
                            },

                            new TestTask
                            {
                                Name = "Running Tests"
                            }
                        },
                    }
                }
            };

            client.SerializeAndWriteToFile(adoPipeline: gitHubPipeline,
                path: "../../../../.github/workflows/dotnet.yml");
        }
    }
}