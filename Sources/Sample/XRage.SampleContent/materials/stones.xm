<material effect="parallax_normal_map">
  
  <sampler semantic="DIFFUSE_MAP">stones/stones_diffuse</sampler>
  <sampler semantic="NORMAL_MAP">stones/stones_height</sampler>
  <sampler semantic="HEIGHT_MAP">stones/stones_height</sampler>
  
  <variable_matrix semantic="WORLDVIEWPROJECTION" 
                   varID="WorldViewProjection"/>
  <variable_matrix semantic="WORLD" 
                   varID="World"/>
  <variable_float3 semantic="CAMERA_POSITION"
                   varID="CameraPosition"/>
  <variable_float semantic="FARCLIPPLANE"
                   varID="FarClipPlane"/>

  <constant_float semantic="SPECULAR_POWER">5</constant_float>
  <constant_float semantic="SPECULAR_INTENSITY">10</constant_float>
  <constant_float semantic="NORMAL_MAP_COEFFICIENT">0.25</constant_float>
  
  <constant_float semantic="PARALLAX_MAP_SCALE">-0.025</constant_float>
  
</material>
