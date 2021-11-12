namespace Widget
{
    public interface IDamageComponent
    {
        bool CanApplyDamage();
        int GetDamageToApply();
    }
}