#pragma once

BEGIN_X_NAMESPACE 
	
public ref class PhysXPhysicsManager : public PhysicsManager
{
public:
	// Default constructor
	PhysXPhysicsManager(XGame^ game);

	// Default destructor
	~PhysXPhysicsManager();

	// Creates a new physics scene.
	virtual IPhysicsScene^ CreateScene() override;

private:
	NxPhysicsSDK*	physXSDK;
};

END_X_NAMESPACE