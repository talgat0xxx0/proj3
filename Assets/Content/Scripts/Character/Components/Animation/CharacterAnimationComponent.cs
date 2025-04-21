public class CharacterAnimationComponent : IAnimationComponent
{
    private Character character;


    public void Initialize(Character character)
    {
        this.character = character;
    }

    public void SetAnimation(string animationName)
    {
        character.CharacterData.Animator.Play(animationName);
    }

    public void SetTrigger(string triggerName)
    {
        character.CharacterData.Animator.SetTrigger(triggerName);
    }

    public void SetBool(string boolName, bool status)
    {
        character.CharacterData.Animator.SetBool(boolName, status);
    }

    public void SetValue(string valueName, float value)
    {
        character.CharacterData.Animator.SetFloat(valueName, value);
    }
}
