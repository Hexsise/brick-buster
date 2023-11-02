using System.Linq;

public class MultiBall : Collectable
{
    protected override void ApplyEffect()
    {
        // ctrl + "." to implement new method and "f12" to go to that method
        foreach (Ball ball in BallsManager.Instance.Balls.ToList())
        {
            BallsManager.Instance.SpawnBalls(ball.gameObject.transform.position, 2, ball.isLightningBall, ball);
        }

    }
}
