namespace Common.Repository;

/// <summary>
/// Wird wahrscheinlich nicht benötigt
/// </summary>
public interface IEntity
{
    int Id { get; set; }
}

/// <summary>
/// Wird wahrscheinlich nicht benötigt
/// </summary>
public interface IEntity<T>
{
    T Id { get; set; }
}
