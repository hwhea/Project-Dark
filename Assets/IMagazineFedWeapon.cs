public interface IMagazineFedWeapon
{
    void OnMagazineEnter();
    void OnMagazineEject();

    void OnMagazineEmpty();

    void OnTriggerPulled();
}