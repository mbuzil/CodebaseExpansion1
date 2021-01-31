
public class PlayerProjectileCollision : ProjectileCollision {
    protected override void Awake() {
        int increases = 0;
        if (PlayerState.Instance.HasUpgrade(PlayerState.PlayerUpgrade.TomeOfWisdom)) {
            increases = PlayerState.Instance.UpgradesCollection[PlayerState.PlayerUpgrade.TomeOfWisdom];
        }

        this.Damage = (int) (this.Damage + 2 * (increases));
    }
}