#ifndef _COMMON_H
#define _COMMON_H

#define TEXTURE_SAMPLER(texture, addressMode, filter) sampler_state { Texture = <texture>; AddressU = addressMode; AddressV = addressMode; MagFilter = filter; MinFilter = filter; Mipfilter = filter; };

#define TEXTURE_SAMPLER_LINEAR(texture, addressMode) sampler_state { Texture = <texture>; AddressU = addressMode; AddressV = addressMode; MagFilter = LINEAR; MinFilter = LINEAR; Mipfilter = LINEAR; };

#define TEXTURE_SAMPLER_POINT(texture, addressMode) sampler_state { Texture = <texture>; AddressU = addressMode; AddressV = addressMode; MagFilter = POINT; MinFilter = POINT; Mipfilter = POINT; };

#define TEXTURE_SAMPLER_ANISOTROPIC(texture, addressMode, maxAnisotropy) sampler_state { Texture = <texture>; AddressU = addressMode; AddressV = addressMode; MagFilter = ANISOTROPIC; MinFilter = ANISOTROPIC; Mipfilter = POINT; MaxAnisotropy = maxAnisotropy; };

#endif