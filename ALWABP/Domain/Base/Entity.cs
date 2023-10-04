namespace ALWABP.Domain.Base
{
    public abstract class Entity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Entity(int id, string name = "")
        {
            Id = id;
            Name = name;
        }
    }
}