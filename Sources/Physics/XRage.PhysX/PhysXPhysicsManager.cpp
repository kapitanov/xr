#pragma unmanaged
#include "NxPhysics.h"

#pragma managed
using namespace System;
using namespace System::Collections::Generic;
using namespace AISTek::XRage;
using namespace Microsoft::Xna::Framework;

#include "Common.h"
#include "PhysXPhysicsManager.h"
#include "PhysXScene.h"

BEGIN_X_NAMESPACE 

// Default constructor
PhysXPhysicsManager::PhysXPhysicsManager(XGame^ game) 
	: PhysicsManager(game)
{
	// Initialize PhysX SDK
	physXSDK = NxCreatePhysicsSDK(NX_PHYSICS_SDK_VERSION);

	if(physXSDK == NULL)
	{
		throw gcnew Exception(L"Failed to initialize PhysX run-time.  Please make sure the latest version of the PhysX System Software is installed.");
	}

	// Default parameters
	physXSDK->setParameter(NX_SKIN_WIDTH, 0.01f);
}

// Default destructor
PhysXPhysicsManager::~PhysXPhysicsManager()
{
	if(physXSDK != NULL)
	{
		physXSDK->release();
	}
}


// Creates a new scene
IPhysicsScene^ PhysXPhysicsManager::CreateScene()
{
	return gcnew PhysXScene(physXSDK);
}

END_X_NAMESPACE

