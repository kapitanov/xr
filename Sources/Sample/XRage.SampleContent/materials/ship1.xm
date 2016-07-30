<material effect="normal_specular_map">
  <sampler semantic="DIFFUSE_MAP">ship1_c</sampler>
  <sampler semantic="NORMAL_MAP">ship1_n</sampler>
  <sampler semantic="SPECULAR_MAP">ship1_s</sampler>
  
  <variable_matrix semantic="WORLDVIEWPROJECTION" 
                   varID="WorldViewProjection"/>
  <variable_matrix semantic="WORLD" 
                   varID="World"/>
  <variable_float semantic="FARCLIPPLANE"
                   varID="FarClipPlane"/>

  <constant_float semantic="SPECULAR_POWER_COEFFICIENT">1</constant_float>
  <constant_float semantic="SPECULAR_INTENSITY_COEFFICIENT">1</constant_float>
  <constant_float semantic="NORMAL_MAP_COEFFICIENT">1</constant_float>
</material>
