namespace JwtStore.Core.Contexts.SharedContext.Entities;

public abstract class Entity : IEquatable<Guid>
{
  protected Entity() 
    => Id = Guid.NewGuid();

  public Guid Id { get; }

  //Compare one entity with another
  public bool Equals(Guid id) 
    => Id == id;
  public override int GetHashCode() 
    => Id.GetHashCode();
}