public class GameConfig{

	int iSpeed;
	int iInitBalls;
	float fShootWait;

	public GameConfig(){
		iSpeed = 15;
		iInitBalls = 50;
		fShootWait = 0.05F;
	}

	public int GetSpeed(){ return iSpeed; }

	public int GetInitBalls(){ return iInitBalls; }

	public float GetShootWait() { return fShootWait; }
}
