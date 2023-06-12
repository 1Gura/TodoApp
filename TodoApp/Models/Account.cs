namespace TodoApp.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }

        public int Status { get; set; }

        public LifeTimeAccountModel OpenedDate { get; set; }    
        public LifeTimeAccountModel ClosedDate { get; set; }    

        public int AccessLevel { get; set; }    
    }
}
