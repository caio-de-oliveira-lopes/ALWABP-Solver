namespace ALWABP.Domain
{
    public abstract class Entity
    {
        private int Id { get; set; }
        private string Name { get; set; }

        public Entity(int id, string name = "") 
        {
            Id = id;
            Name = name;
        }
    }
}