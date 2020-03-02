using System.Collections.Generic;
using System.Linq;
using k8s;
using k8s.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace k8s_client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly ILogger<object> _logger;
        private KubernetesClientConfiguration kubeConfig;
        private Kubernetes kubeClient;

        public JobsController(ILogger<object> logger)
        {
            _logger = logger;
            kubeConfig = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            kubeClient = new Kubernetes(kubeConfig);
        }

        [HttpGet]
        public IEnumerable<V1Job> Get()
        {
            var jobs = kubeClient.ListJobForAllNamespaces();
            return jobs.Items;
        }

        [HttpGet("{uid}")]
        public V1Job GetByUId(string uid)
        {
            var jobs = kubeClient.ListJobForAllNamespaces();
            return jobs.Items.Where(w => w.Metadata.Uid.StartsWith(uid)).SingleOrDefault();
        }

        [HttpPost]
        public IActionResult Post([FromQuery] string name, string namespaceName)
        {
            var job = new V1Job
            {
                ApiVersion = "batch/v1",
                Kind = "Job",
                Metadata = new V1ObjectMeta
                {
                    Name = name
                },
                Spec = new V1JobSpec
                {
                    Template = new V1PodTemplateSpec()
                    {
                        Spec = new V1PodSpec()
                        {
                            Containers = new List<V1Container>()
                            {
                                new V1Container()
                                {
                                    Name = "container-test",
                                    Image = "hello-world"
                                }
                            }, 
                            RestartPolicy = "Never"
                        }
                    }
                }
            };

            var result = kubeClient.CreateNamespacedJob(job, namespaceName);
            return Ok();
        }

        public IActionResult Delete([FromQuery] string name, string namespaceName)
        {
            var result = kubeClient.DeleteNamespacedJob(name, namespaceName);
            return Ok();
        }
    }
}
