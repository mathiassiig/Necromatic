using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;


// based on https://github.com/sharpaccent/Astar-for-Unity/blob/master/Assets/Scripts/PathfindMaster.cs
namespace Necromatic.World
{
    public class PathfinderManager
    {
        public int MaxJobs = 3;

        private List<Pathfinder> currentJobs;
        private List<Pathfinder> todoJobs;

        public PathfinderManager()
        {
            currentJobs = new List<Pathfinder>();
            todoJobs = new List<Pathfinder>();
        }

        public void Init()
        {
            Observable.EveryUpdate().TakeUntilDestroy(GameManager.Instance).Subscribe(_ =>
            {
                int i = 0;

                while(i < currentJobs.Count)
                {
                    if(currentJobs[i].PathFound != null)
                    {
                        currentJobs.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                if(todoJobs.Count > 0 && currentJobs.Count < MaxJobs)
                {
                    Pathfinder job = todoJobs[0];
                    todoJobs.RemoveAt(0);
                    currentJobs.Add(job);

                    Thread jobThread = new Thread(job.StartJob);
                    jobThread.Start();
                }
            });
        }

        public ReactiveProperty<List<Node>> RequestPathfind(Vector3 start, Vector3 end)
        {
            var startNode = GameManager.Instance.NavMesh.GetNode(start);
            var endNode = GameManager.Instance.NavMesh.GetNode(end);
            return RequestPathfind(startNode, endNode);
        }

        public ReactiveProperty<List<Node>> RequestPathfind(Node start, Node target)
        {
            Pathfinder newJob = new Pathfinder(start, target);
            todoJobs.Add(newJob);
            return newJob.PathFound;
        }
    }
}
