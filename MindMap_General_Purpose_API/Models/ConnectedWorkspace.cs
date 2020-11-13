namespace MindMap_General_Purpose_API.Controllers
{
    public class ConnectedWorkspace
    {
        public ConnectedWorkspace()
        {

        }

        public ConnectedWorkspace(string workspaceId)
        {
            WorkspaceId = workspaceId;
        }

        public string WorkspaceId { get; set; }
    }
}
