/*
* IInteractable.cs
* The contract now requires the Interact method
* to accept a PlayerStats component from the interactor.
*/
public interface IInteractable
{
    // The "PlayerStats stats" parameter is new
    void Interact(PlayerStats stats);
    string InteractionPrompt {  get; }
}