#pragma once

BEGIN_X_NAMESPACE 

public ref class PhysXScene : public IPhysicsScene
{
internal:
	PhysXScene(NxPhysicsSDK* sdk);

	~PhysXScene();

public:
	property Vector3 Gravity
	{
		virtual Vector3 get()
		{
			NxVec3 grav;
			physXScene->getGravity(grav);

			return NXVEC3_TO_VECTOR3(grav);
		};

		virtual void set(Vector3 value)
		{
			physXScene->setGravity(VECTOR3_TO_NXVEC3(value));
		};
	};

	virtual IPhysicsActor^ CreateActor(ActorDesc^ desc);

	virtual void BeginFrame(Microsoft::Xna::Framework::  GameTime^ gameTime);

	virtual void EndFrame();

private:
	NxPhysicsSDK*	physXSDK;
	NxScene*		physXScene;
};

END_X_NAMESPACE