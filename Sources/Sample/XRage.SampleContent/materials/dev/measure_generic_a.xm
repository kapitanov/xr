<material effect="temporary">
  <sampler semantic="DIFFUSE_MAP">dev/measure_generic_a</sampler>
  <sampler semantic="NORMAL_MAP">dev/bump</sampler>
  <sampler semantic="SPECULAR_MAP">dev/bump</sampler>
  
  <variable_matrix semantic="WORLDVIEWPROJECTION" 
                   varID="WorldViewProjection"/>
  <variable_matrix semantic="WORLD" 
                   varID="World"/>

  <constant_float semantic="SPECULAR_POWER_COEFFICIENT">1</constant_float>
  <constant_float semantic="SPECULAR_INTENSITY_COEFFICIENT">1</constant_float>
</material>
