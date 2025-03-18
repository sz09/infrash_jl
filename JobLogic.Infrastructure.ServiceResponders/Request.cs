namespace JobLogic.Infrastructure.ServiceResponders
{
    public enum ActionType
    {
        Query,
        Insert,
        Update,
        Delete,
        Assign,
        Add,
        Edit,
        Save
    }
    public class BaseRequest<T>
    {
    }

    public class Request<T>
    {
        public T Data { get; set; }
        public ActionType Action { get; set; }

        public Request(T data)
        {
            Data = data;
        }

        public Request()
        {

        }
    }
}
