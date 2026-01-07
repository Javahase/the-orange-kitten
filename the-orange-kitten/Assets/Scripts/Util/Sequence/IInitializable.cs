/// <summary>
/// An <c>IInitializable</c> represents something that can be
/// initialized, enabled and disabled. Certain controllers
/// make use of objects of this type.
/// </summary>
public interface IInitializable
{
    /// <summary>
    /// Called upon the initialization step of this object.
    /// </summary>
    void OnInitialize();
    
    /// <summary>
    /// Called when this object is being enabled.
    /// </summary>
    void OnEnabled();
    
    /// <summary>
    /// Called when this object is being disabled.
    /// </summary>
    void OnDisabled();
}
