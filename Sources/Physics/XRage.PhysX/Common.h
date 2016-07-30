#pragma once

#define BEGIN_X_NAMESPACE namespace AISTek { namespace XRage { namespace Physics {

#define END_X_NAMESPACE } } };

// Conversion macros for XNA Vector3 and PhysX NxVec3
#define NXVEC3_TO_VECTOR3(vec) Vector3(vec.x, vec.y, vec.z)

#define VECTOR3_TO_NXVEC3(vec) NxVec3(vec.X, vec.Y, vec.Z)