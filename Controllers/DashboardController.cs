namespace Rokono_Control.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Rokono_Control.DatabaseHandlers;
    using Rokono_Control.Models;
    using RokonoControl.Models;

    public class DashboardController : Controller
    {
        RokonoControlContext Context;
        public DashboardController(RokonoControlContext context)
        {
            Context = context;
        }
        #region PageRenders
        public IActionResult Index()
        {
            var currentUser = this.User;
            var id = currentUser.Claims.ElementAt(1);         
            using(var context = new DatabaseController(Context))
            {
                var currentId = int.Parse(id.Value);
                ViewData["Projects"] = context.GetUserProjects(currentId);
                ViewData["Name"] = context.GetUsername(currentId);
                var rights = currentUser.Claims.LastOrDefault().Value;
                ViewData["IsAdmin"] = int.Parse(rights) == 1 ? true : false;
            }
            return View();
        }
        public IActionResult AddNewProject(string user)
        {
            ViewData["User"] = user;
            return View();
        }
        public IActionResult AddNewWorkItem(int projectId, int workItemType, int parentId)
        {     
            var currentUser = this.User;
            using(var context = new DatabaseController(Context))
            {
                var rights = currentUser.Claims.LastOrDefault().Value;
                ViewData["IsAdmin"] = int.Parse(rights) == 1 ? true : false;
                ViewData["Priorities"] = context.GetProjectPriorities(projectId); 
                ViewData["Areas"] = context.GetProjectAreas(projectId);
                ViewData["Iterations"] = context.GetProjectIterations(projectId);
                ViewData["Severities"] = context.GetProjectSeverities(projectId);
                ViewData["Activities"] = context.GetProjectActivities(projectId);
                ViewData["Reasons"] = context.GetProjectReasons(projectId);
                ViewData["Builds"] = context.GetProjectBuilds(projectId);
                ViewData["Relationships"] = context.GetProjectRelationships();
                ViewData["ValueAreas"] = context.GetProjectValueAreas(projectId);
                ViewData["Risks"] = context.GetProjectRisks(projectId);
                ViewData["WorkItemType"] = workItemType;
                ViewData["ProjectId"] = projectId;
                ViewData["ParentId"] = parentId;
            }
            return View();
        }

        public IActionResult EditWorkItem(int projectId, int workItem)
        {
            var currentUser = this.User;
            using(var context = new DatabaseController(Context))
            {
                var defaultUserAccount = context.GetDefaultAccount();
                var rights = currentUser.Claims.LastOrDefault().Value;
                ViewData["IsAdmin"] = int.Parse(rights) == 1 ? true : false;
                var workItemData = context.GetWorkItem(workItem, projectId); 
                workItemData.DueDate = workItemData.DueDate.HasValue ? workItemData.DueDate.Value : new System.DateTime(); 
                workItemData.StartDate = workItemData.StartDate.HasValue ? workItemData.StartDate.Value : new System.DateTime(); 
                workItemData.EndDate = workItemData.EndDate.HasValue ? workItemData.EndDate.Value : new System.DateTime(); 
                workItemData.AssignedAccountNavigation  = workItemData.AssignedAccountNavigation == null ? defaultUserAccount  : workItemData.AssignedAccountNavigation;
                var currentWorkItem = workItemData;
                ViewData["Priorities"] = context.GetProjectPriorities(projectId); 
                ViewData["Areas"] = context.GetProjectAreas(projectId);
                ViewData["Iterations"] = context.GetProjectIterations(projectId);
                ViewData["Severities"] = context.GetProjectSeverities(projectId);
                ViewData["Activities"] = context.GetProjectActivities(projectId);
                ViewData["Reasons"] = context.GetProjectReasons(projectId);
                ViewData["Builds"] = context.GetProjectBuilds(projectId);
                ViewData["Relationships"] = context.GetProjectRelationships();
                ViewData["ValueAreas"] = context.GetProjectValueAreas(projectId);
                ViewData["Risks"] = context.GetProjectRisks(projectId);
                ViewData["WorkItemType"] = currentWorkItem.WorkItemTypeId;
                ViewData["WorkItemData"] = currentWorkItem;
                ViewData["ProjectId"] = projectId;
             }
            return View();
        }

        public IActionResult ManageAccounts()
        {
            var currentUser = this.User; 
            using(var context = new DatabaseController(Context))
            {
                var rights = currentUser.Claims.LastOrDefault().Value;
                ViewData["IsAdmin"] = int.Parse(rights) == 1 ? true : false;
                ViewData["Relationships"] = context.GetProjectRelationships();
                ViewData["UserAccounts"] = context.GetUserAccounts();
            }
            return View();
        }

        public IActionResult ProjectDetails()
        {
            return View();
        }

        public IActionResult EditAccount(int id)
        {
            var currentUser = this.User;

            using(var context = new DatabaseController(Context))
            {
                var rights = currentUser.Claims.LastOrDefault().Value;
                ViewData["IsAdmin"] = int.Parse(rights) == 1 ? true : false;
                ViewData["Relationships"] = context.GetProjectRelationships();
                ViewData["UserAccount"] = context.GetSpecificUserEdit(id);
                ViewData["UserId"] = id;
            }
            return View();
        }
        public IActionResult AddNewAccount()
        {
            var currentUser = this.User;

            using(var context = new DatabaseController(Context))
            {
                var rights = currentUser.Claims.LastOrDefault().Value;
                ViewData["IsAdmin"] = int.Parse(rights) == 1 ? true : false;
                ViewData["Relationships"] = context.GetProjectRelationships();
    
            }
            return View();
        }
        public IActionResult AssignAccountProjects(int id)
        {
            var currentUser = this.User;
            using(var context = new DatabaseController(Context))
            {
                var rights = currentUser.Claims.LastOrDefault().Value;
                ViewData["IsAdmin"] = int.Parse(rights) == 1 ? true : false;
                ViewData["Relationships"] = context.GetProjectRelationships();
                ViewData["UserAccount"] = context.GetSpecificUserEdit(id);
                ViewData["UserId"] = id;
            }
            return View();
        }


        public IActionResult ProjectDashboard(int id)
        {
            var currentUser = this.User;

            using(var context = new DatabaseController(Context))
            {
                var rights = currentUser.Claims.LastOrDefault().Value;
                ViewData["IsAdmin"] = int.Parse(rights) == 1 ? true : false;
                ViewData["Relationships"] = context.GetProjectRelationships();
                ViewData["Project"] = context.GetProjectData(id);
                ViewData["ProjectMembers"] = context.GetProjectMembers(id);
                ViewData["ProjectId"] = id;
              
            }
            return View();
        }

        public IActionResult WorkItems(int projectId, int boardId)
        {
            var currentUser = this.User;
            var id = currentUser.Claims.ElementAt(1);         
            using(var context = new DatabaseController(Context))
            {
                var rights = currentUser.Claims.LastOrDefault().Value;
                ViewData["IsAdmin"] = int.Parse(rights) == 1 ? true : false;
                var currentId = int.Parse(id.Value);
                ViewData["ProjectId"] = projectId;
                ViewData["Relationships"] = context.GetProjectRelationships();
                ViewData["Name"] = context.GetUsername(currentId);
                ViewData["BoardId"] = boardId;
            }
            return View();
        }

        #endregion
        
        #region AjaxRequests
        [HttpGet]
        public List<WorkItemRelations> GetWorkItemRelations() 
        {
            var relationships = new List<WorkItemRelations>();
            using(var context = new DatabaseController(Context))
                relationships = context.GetProjectRelationships();
            return relationships;
        }
        [HttpGet]
        public OutgoingBoundRelations GetAllWorkItemRelations(int workItemId, int projectId)
        {
            var result = default(OutgoingBoundRelations);
            using(var context = new DatabaseController(Context))
            {
                result = context.GetAllWorkItemRelations(workItemId,projectId);
            }
            return result;
        }
        [HttpGet]
        public List<OutgoingBindingWorkItem> GetAllWorkItems(int projectId)
        {
            var result = new List<OutgoingBindingWorkItem>();
            using(var context = new DatabaseController(Context))
            {
                result = context.GetAllWorkItems(projectId);
            }
            return result;
        }

        [HttpPost]
        public List<CommitChartBindingData> GetCommitChartBindingData([FromBody] IncomingIdRequest request)
        {
            var result = new List<CommitChartBindingData>();
            using(var context = new DatabaseController(Context))
            {
                result = context.GetCommitsChartForProject(request.Id);
            }
            return result;
        }

        [HttpGet]
        public object GetProjects(){
            var result = default(object);
            using(var context = new DatabaseController(Context))
            {
                result = context.GetProjects();
            }
            return  result;
        }
      


        [HttpPost]
        public OutgoingProjectRules GetProjectUserRules([FromBody] IncomingProjectRulesRequest project)
        {
            var getProjectRules = default(OutgoingProjectRules);
            using(var context = new DatabaseController(Context))
            {
                getProjectRules = context.GetProjectRules(int.Parse(project.ProjectId),int.Parse(project.UserId));
            }
            return getProjectRules;
        }

        [HttpPost]
        public void UpdatePACommitRule([FromBody] IncomignRuleUpdate projectRuleData)
        {
            using(var context = new DatabaseController(Context))
            {
                context.UpdateProjectUserRule(projectRuleData,"CommitRule");
            }
        }

        [HttpPost]
        public void UpdatePACloneRule([FromBody]  IncomignRuleUpdate projectRuleData)
        {
            using(var context = new DatabaseController(Context))
            {
                context.UpdateProjectUserRule(projectRuleData,"CloneRule");
            }
        }

        [HttpPost]
        public void UpdatePAViewWorkRule([FromBody]  IncomignRuleUpdate projectRuleData)
        {
            using(var context = new DatabaseController(Context))
            {
                context.UpdateProjectUserRule(projectRuleData,"ViewWorkRule");

            }
        }

        [HttpPost]
        public void UpdatePACreateWorkRule([FromBody]  IncomignRuleUpdate projectRuleData)
        {
            using(var context = new DatabaseController(Context))
            {
                context.UpdateProjectUserRule(projectRuleData,"CreateWorkRule");
               
            }
        }

        [HttpPost]
        public void UpdatePADeleteWorkRule([FromBody]  IncomignRuleUpdate projectRuleData)
        {
            using(var context = new DatabaseController(Context))
            {
                context.UpdateProjectUserRule(projectRuleData,"DeleteWorkRule");   
            }
        }

        [HttpPost]
        public bool StageWorkItem([FromBody] IncomingWorkItem currentItem)
        {
            var result = default(bool);
         

            using(var context = new DatabaseController(Context))
            {
                result=context.AddNewWorkItem(currentItem);
            }
            return result;
        }
        [HttpPost]
        public bool UpdateWorkItem([FromBody] IncomingWorkItem currentItem)
        {
            var result = default(bool);
            using(var context = new DatabaseController(Context))
            {
                result=context.UpdateWorkItem(currentItem);
            }
            return result;
        }
        [HttpPost]
        public List<BindingUserAccount> GetUserAccounts([FromBody] IncomingIdRequest IncomingIdRequest)
        {
            var  result =  new List<BindingUserAccount>();
            using(var context = new DatabaseController(Context)){
                result =  context.GetProjectMembers(IncomingIdRequest.Id);
            }
            return result;

        }
         [HttpPost]
        public bool RemoveUserFromProject([FromBody] IncomingRemoveUserFromProject userProject)
        {
            using(var context = new DatabaseController(Context))
            {
                context.RemoveUserFromProject(userProject);
            }
            return true;

        }
        
        
        [HttpPost] 
        public OutgoingValidWorkItem ValidateSelectedItem([FromBody] IncomignWorkItemRelation incomingRequest)
        {
            
            var result = new OutgoingValidWorkItem();
          
            using(var context = new DatabaseController(Context))
            {
                var defaultUserAccount = context.GetDefaultAccount();
                result.WorkItem = new List<WorkItem>();
                if(!incomingRequest.LinkedItems.Any(x=>x.WorkItemId == incomingRequest.WorkItemId))
                {
                    if(incomingRequest.CurrWorkItemId != 1)
                    {
                        var workItemData = context.GetWorkItemChildrenClean(incomingRequest.CurrWorkItemId); 
                        var currentWorkItem = context.GetWorkItemClean(incomingRequest.CurrWorkItemId, incomingRequest.ProjectId);
                        if(currentWorkItem.ParentId != null )
                        {
                            if(currentWorkItem.ParentId != 0)
                                result.WorkItem.Add(context.GetWorkItemClean(currentWorkItem.ParentId.Value, incomingRequest.ProjectId));
                        }
                        result.WorkItem = workItemData;

                    }
                    incomingRequest.LinkedItems.ForEach(x=>{
                        result.WorkItem.Add(context.GetWorkItemClean(x.WorkItemId, incomingRequest.ProjectId));
                    });
                    result.Valid = true;
                    result.WorkItem.Add(context.GetWorkItemClean(incomingRequest.WorkItemId, incomingRequest.ProjectId));
                    result.WorkItemTypeId = int.Parse(incomingRequest.RelationType);
                    result.WorkItemId = incomingRequest.WorkItemId;
                    result.RelationshipId = incomingRequest.RelationType;
                }
            }
            return result;
        }

        [HttpPost]
        public bool AssociatedWorkItemRelation([FromBody] IncomignWorkItemRelation incomingRelation)
        {
            using(var context = new DatabaseController(Context))
            {
                context.AssociatedRelation(incomingRelation);
            }
            return true;
        }

        [HttpPost] 
        public string AddProjectToUserAccount([FromBody] IncomingProjectUser incomingRequest)
        {          
            using(var context = new DatabaseController(Context))
            {
                context.AddProjectToUser(incomingRequest);
            }
            return "true";
        }

        [HttpPost]
        public bool AddNewProject([FromBody] IncomingProject currentProject)
        {
            var result = default(bool);
            using(var context = new DatabaseController(Context))
            {
                var currentUser = this.User;
                var userDto = currentUser.Claims.ElementAt(1);         
                result=context.AddNewProject(currentProject,int.Parse(userDto.Value));
            }
            return result;
        }

        #endregion
    }
}