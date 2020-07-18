
--------------------------------------------------------------------------------
	Contents
--------------------------------------------------------------------------------

- Introduction & Contact
- License
- Contents
- Krita
- Modifications
- Island Generator

--------------------------------------------------------------------------------
	Introduction & Contact
--------------------------------------------------------------------------------

Hey there!

Thank you a lot for buying ~ Isle of Lore Hex Tiles ~! The asset pack
consists of two parts: the hex tile graphics you can use for your games or
print projects, and an island generator which generates little islands.
Depending on where you bought the assets, the generator may not be included
(because selling software over an asset store can be difficult at times!).
If that's the case and you want to play around with the island generator,
please let me know, I can send you a copy!

Also, if you encounter any problem, got a question or have some feedback,
you can reach me easily via mail (see below).

By the way, I may update this asset pack (at no additional cost) in the future
and if you want to keep up-to-date and learn more about my other game-related
projects, you can subscribe to my mailing list or join my Discord.

	Mailing List: 	http://mailinglist.stevencolling.com/game_assets/
	Discord:		http://discord.stevencolling.com
	Mail:			info@stevencolling.com
	Website:		http://www.stevencolling.com

See you there!
Steven Colling

--------------------------------------------------------------------------------
	License
--------------------------------------------------------------------------------

Depending on where you bought ~ Isle of Lore Hex Tiles ~, the store may provide
license terms for the assets they sell, including this one. If that's not the
case, you'll find a "License.txt" file in the topmost directory. Please refer
to either the storefront's asset license or to the license file provided with
the assets. If you think the seller's license is too restricting for your usage,
please reach out via mail: info@stevencolling.com.

--------------------------------------------------------------------------------
	Contents
--------------------------------------------------------------------------------

Isle of Lore Hex Tiles contains...

- 20x Forest Tiles
- 20x Grassland Tiles
- 20x Mountain Tiles
- 20x Ocean Tiles
- 20x Ocean Tiles, but with bigger waves drawn onto them
- Overlays for hex borders, hex corners, paths, roads, rivers and shorelines
- 200x Conifer Tree Elements
- 200x Deciduous Tree Elements
- 200x Short Grass Elements
- 100x Long Grass Elements
- 200x Rock Elements
- 20x Mountain Elements
- 100x Ocean Line Elements
- 100x Ocean Wave Elements
- 30x Locations
- 30x Abstract Icons
- Island Generator Program

...in the following ways:

- png files in colored and grayscale variant
- source files as Krita (kra) and Photoshop (psd, exported from Krita)
- as single image files and sprite sheets

There are 5 types of files in the asset pack, namely:

- Tiles:		hexagon tiles, including forest, mountain, grassland
				and ocean tiles
- Overlays:		tiles like roads or rivers which can be placed on top of
				a tile, for example to create a forest with a river
				running through
- Elements:		little graphics from which the tiles were compiled, like
				single trees, rocks or mountains; with these elements, you
				can create tiles easily by yourself
- Locations:	icons indicating locations like a village or witch hut
- Icons:		abstract icons in a white circle to add more information
				to a map if necessary, for example indicating how dangerous
				a tile is with one of the skull icons

If you have a look at the directories, they are structured like this:

./					top level directory

README.txt			this file
License.txt			see "License" section above
Changelog.txt		logging the asset pack's changes
Showcase.png		an example on how the assets can look like in usage
Showcase.kra		the Krita source file of Showcase.png
Showcase.psd		psd export of the Showcase source file

./Images			tiles, overlays, elements, locations and icons as single pngs
./Images/Colored	the files in the colored variant in their own directories
./Images/Grayscale	the files in the grayscale variant in their own directories

./Sources			Krita and psd source files on which the files in ./Images
					are based on; the sub-directories contain the source files
					for every single tile; the source files of the overlays,
					elements, locations and icons are stored in ./Sources
					itself;
					there are two special files template_hex.kra and
					template_map.kra with their corresponding pngs which
					include a template hex tile which was used to create
					all of the tiles as well as an exemplary hex grid
					where you could assemble your own hex map

./Sprite Sheets		all image files compiled into sprite sheets, either
					1024x1024px or 2048x2048px in total size

./Generator			island generator (see "Island Generator" section below)

--------------------------------------------------------------------------------
	Krita
--------------------------------------------------------------------------------

The files were originally made with the free (and amazing) painting software
Krita, available at krita.org. There are also Photoshop exports available. If
you encounter a problem with those exports, please let me know!

--------------------------------------------------------------------------------
	Modifications
--------------------------------------------------------------------------------

Warning: if you make changes to the files, please copy the files to a different
location first, before you download a new version of the assets and hence
accidentally overwrite your modifications!

--------------------------------------------------------------------------------
	Island Generator
--------------------------------------------------------------------------------

The Island Generator generates little islands as png files. If you run it the
first time, it will create a Configuration.txt file, as well as an Output folder.
If you move the Island Generator, you will have to set the InputDirectory to the
Images directory of the asset pack, for example:

.\Isle of Lore Hex Tiles\Images\Colored

You can also store the configuration somewhere else and pass the configuration
file's location as a program argument.

The configurable options are shown below. Chances are given as floating numbers,
meaning 0.15 stands for 15%. Tokens are used to determine relative probabilities.
If, for example, TokensGrassLand is set to 1000 and TokensForest to 2000, forests
are double as likely than grasslands.

BackgroundColor					color of the island's background (it's set to the ocean's color)
InputDirectory					where to find the images from the asset pack (e.g. .\Images\Colored)
OutputDirectory					where to store the generated file (if empty, they are stored in .\Output at the generator's location)
OverwriteOutputFile				if true, the generator will overwrite the created island file with further runs, instead of creating more files
GridSize						size of the hex grid (the image output won't work with a grid too big)
TileSize						size of a tile in pixels
TileWidthCenter					width of the tile's bottom horizontal border
TileWidthAngle					width of the tile's corner
TilePadding						additional padding around the tile (open a tile from the asset pack as reference)
TileOffset						additional offset for tiles placed
TileToCenterOffset				offset from the top left corner to the tile's center
CanvasPadding					padding between the whole island and the final image's border
ChanceForOceansWithWaves		chance for swapping an ocean tile without waves with an ocean tile with waves
TokensGrassland					relative chance (tokens) for grassland tiles
TokensForest					relative chance (tokens) for forest tiles
TokensMountain					relative chance (tokens) for mountain tiles
TokensInlandOcean				relative chance (tokens) that a tile which should be land is made into an ocean instead (the border around the island is always ocean)
ChancePerRiver					if RiverMaxCount is set to 2, each of those two rivers can appear with this chance
RiverMaxCount					the maximal count of rivers which can (but don't have to) appear
RiverMaxLength					maximal length of rivers (shorter rivers are possible)
RiverEndingChance				after the generation of a new river piece, the river can end before reaching maximal length with this chance
ChancePerRoad					compare to ChancePerRiver
ChanceForPath					chance that a road is turned into a path
RoadMaxCount					compare to RiverMaxCount
RoadMinLength					minimal length of a road or path
RoadMaxLength					maximal length of a road or path (compare to RiverMaxLength)
RoadEndingChance				compare to RiverEndingchance
ChanceForRoadAvoidingOcean		chance that the next road or path piece prefers a route away from the ocean
ChanceForMaxingRoadDistance		chance that the next road or path piece prefers a route which maximizes the distance to the road or path's starting point