<material effect="parallax_normal_specular_map">
  
  <sampler semantic="DIFFUSE_MAP">ground_diffuse</sampler>
  <sampler semantic="NORMAL_MAP">ground_normal</sampler>
  <sampler semantic="SPECULAR_MAP">ground_specular</sampler>
  <sampler semantic="HEIGHT_MAP">ground_height</sampler>
  
  <variable_matrix semantic="WORLDVIEWPROJECTION" 
                   varID="WorldViewProjection"/>
  <variable_matrix semantic="WORLD" 
                   varID="World"/>
  <variable_float3 semantic="CAMERA_POSITION"
                   varID="CameraPosition"/>

  <constant_float semantic="SPECULAR_POWER_COEFFICIENT">0.1</constant_float>
  <constant_float semantic="SPECULAR_INTENSITY_COEFFICIENT">10</constant_float>
  <constant_float semantic="NORMAL_MAP_COEFFICIENT">0.25</constant_float>
  
  <constant_float2 semantic="PARALLAX_MAP_SCALE_BIAS">1 1</constant_float2>
  
</material>
