<material effect="terrain">
  <sampler semantic="TEXTURE_MAP">terrain/gcanyon_terrain</sampler>

  <sampler semantic="GRASS_TEXTURE">grass</sampler>
  <sampler semantic="GRASS_NORMAL">grassNormal</sampler>

  <sampler semantic="SAND_TEXTURE">sand</sampler>
  <sampler semantic="SAND_NORMAL">sandNormal</sampler>

  <sampler semantic="ROCK_TEXTURE">rock</sampler>
  <sampler semantic="ROCK_NORMAL">rockNormal</sampler>

  <variable_matrix semantic="WORLDVIEWPROJECTION"
                   varID="WorldViewProjection"/>
  <variable_matrix semantic="WORLD"
                   varID="World"/>

  <constant_float semantic="SPECULAR_POWER">1</constant_float>
  <constant_float semantic="SPECULAR_INTENSITY">1</constant_float>
  <constant_float semantic="TERRAIN_SCALE">75</constant_float>

</material>
