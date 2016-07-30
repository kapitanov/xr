<material effect="normal_specular_map">
  <sampler semantic="DIFFUSE_MAP">ground_diffuse</sampler>
  <sampler semantic="NORMAL_MAP">ground_normal</sampler>
  <sampler semantic="SPECULAR_MAP">ground_specular</sampler>
  
  <variable_matrix semantic="WORLDVIEWPROJECTION" 
                   varID="WorldViewProjection"/>
  <variable_matrix semantic="WORLD" 
                   varID="World"/>

  <constant_float semantic="SPECULAR_POWER_COEFFICIENT">2</constant_float>
  <constant_float semantic="SPECULAR_INTENSITY_COEFFICIENT">10</constant_float>
</material>
