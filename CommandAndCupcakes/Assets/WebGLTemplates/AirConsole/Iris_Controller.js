// big daddy air console
var airconsole;

// used to access the various divs holding the UIs
var menuDivs = [];

// all elements that read touch / click
var buttonElements = [];

// dPad buttons
var dPadElements = [];

// all colored footers
var footerElements = [];

// all colored headers
var headerElements = [];

// all head images that should be set for player
var customHeadElements = [];

// all health bar elements
var healthBarElements = [];

// all health text elements
var healthTextElements = [];

var batteryChargeTextElements = [];

// player header name text elements
var playerHeaderNameTextElements = [];

// all action text elements
var actionTextElements = [];

// all options buttons
var optionsButtonElements = [];

// all skill pts text elements
var skillPtsTextElements = [];

// all moves remaining container elements
var movesRemainingContainerElements = [];

// all moves remaining text elements
var movesRemainingTextElements = [];

// player select button elements
var playerSelectButtonElements = []

// colored drop shadow player select elements
var playerSelectShadowElements = [];

// text elements for player select buttons
var playerSelectTextElements = [];

// backout button for character select
var playerBackoutButtonElements = [];

// text used for player descriptions
var playerDescriptionTextElements = [];

// ready triangles at the bottom of the character select screen
var playerReadyTriangleElements = [];

// upgrade ready triangles
var playerUpgradeTriangleElements = [];

// help buttons
var helpButtonElements = [];

// text elements for player stats in the select screen
var playerOneSelectStatTextElements = [];
var playerTwoSelectStatTextElements = [];
var playerThreeSelectStatTextElements = [];
var playerFourSelectStatTextElements = [];

// triangle elements in player select
var playerSelectInnerTriangleElements = [];
var playerSelectOuterTriangleElements = [];

// game over stats
var gameOverStatValueElements = [];

// stat texts for upgrade screen
var upgradeStatTextElements = [];

// elements that should only be active for the master controller
var masterControllerElements = [];

var abilityButtonImages = [];
var abilityButtonText = [];

// images used for the loading animation
var loadingImageSequence = [
	"Images/Loading/loading_left_01.png",
	"Images/Loading/loading_left_02.png",
	"Images/Loading/loading_left_03.png",
	"Images/Loading/loading_left_04.png"
];

// images used for the spectating animation
var spectatingImageSequence = [
	"Images/Spectating/spectating_left_01.png",
	"Images/Spectating/spectating_left_02.png",
	"Images/Spectating/spectating_left_03.png",
	"Images/Spectating/spectating_left_04.png"
];

var buttonForwardAudio = new Audio('Audio/button_forward.wav');
var buttonBackAudio = new Audio('Audio/button_back.wav');

function playButtonForward()
{
	buttonForwardAudio.play();
}

function playButtonBack()
{
	buttonBackAudio.play();
}

// item stuff ---

var ItemType =
{
	PISTOL: 0,
	SNIPER: 1,
	SHOTGUN: 2,
	GRENADELAUNCHER: 3,
	STATPOINT: 4,
	BATTERY: 5,
	REVIVE: 6,
	MINE: 7,
	FREEZERAY: 8,
	UTILITYBELT: 9,
	CLOAK: 10,
	GRENADE: 11
}
var NUM_ITEMS = 12;

var itemNames = 
[
	"PISTOL",
	"SNIPER",
	"SHOTGUN",
	"GRENADE LAUNCHER",
	"STAT POINT",
	"BATTERY",
	"REVIVE",
	"MINE",
	"FREEZE RAY",
	"UTILITY BELT",
	"CLOAK",
	"GRENADE"
];

// icon urls
itemIcons = [
	"Images/Weapon_Icons/weapon_pistol.png",
	"Images/Weapon_Icons/weapon_sniper.png",
	"Images/Weapon_Icons/weapon_shotgun.png",
	"Images/Weapon_Icons/weapon_grenade_launcher.png",
	"Images/Item_Icons/item_skill_point.png",
	"Images/Item_Icons/item_battery_full.png",
	"Images/Item_Icons/item_revive.png",
	"Images/Item_Icons/item_mine.png",
	"Images/Item_Icons/item_freeze_ray.png",
	"Images/Item_Icons/item_utility_belt.png",
	"Images/Item_Icons/item_cloaking_device.png",
	"Images/Item_Icons/item_grenade.png"
];
gunIcons = [
	[
		"Images/Weapon_Icons/Weapon_Pistol/weapon_pistol_0.png",
		"Images/Weapon_Icons/Weapon_Pistol/weapon_pistol_1.png",
		"Images/Weapon_Icons/Weapon_Pistol/weapon_pistol_2.png",
		"Images/Weapon_Icons/Weapon_Pistol/weapon_pistol_3.png"
	],

	[
		"Images/Weapon_Icons/Weapon_Sniper/weapon_sniper_0.png",
		"Images/Weapon_Icons/Weapon_Sniper/weapon_sniper_1.png",
		"Images/Weapon_Icons/Weapon_Sniper/weapon_sniper_2.png",
		"Images/Weapon_Icons/Weapon_Sniper/weapon_sniper_3.png"
	],

	[
		"Images/Weapon_Icons/Weapon_Shotgun/weapon_shotgun_0.png",
		"Images/Weapon_Icons/Weapon_Shotgun/weapon_shotgun_1.png",
		"Images/Weapon_Icons/Weapon_Shotgun/weapon_shotgun_2.png",
		"Images/Weapon_Icons/Weapon_Shotgun/weapon_shotgun_3.png"
	],

	[
		"Images/Weapon_Icons/Weapon_Grenade_Launcher/weapon_grenade_launcher_0.png",
		"Images/Weapon_Icons/Weapon_Grenade_Launcher/weapon_grenade_launcher_1.png",
		"Images/Weapon_Icons/Weapon_Grenade_Launcher/weapon_grenade_launcher_2.png",
		"Images/Weapon_Icons/Weapon_Grenade_Launcher/weapon_grenade_launcher_3.png"
	]
];

// the container parents
var itemContainerElements = [];

// ready triangles at the bottom of the item select screen
var playerItemReadyTriangleElements = [];

// text elements for item names
var itemSelectNameTextElements = [];

// text elements for backout button text
var itemBackoutNameTextElements = [];

// colored drop shadow item select elements
var itemSelectShadowElements = [];

// player select button elements
var itemSelectButtonElements = []

// text used on the player select buttons
var itemBackoutButtonElements = [];

// current device ids for selected items
var itemDevices = [];

// icons for the item select items
var itemIconImageElements = [];

// gun stats
var itemGunStatsContainerElements = [];
var itemGunStatSoloContainerElements = [];
var itemGunStatNameTextElements = [];
var itemGunStatValueTextElements = [];

// text used for item descriptions
var itemDescriptionTextElements = [];

// --------------

// cooldown map
var cooldownActions = [];
var cooldownKeys = [];

// the current iris player index for this controller
var currentIrisIndex = -1;

// the device whose turn it is
var currentTurnDevice = -1;

// are they?
var dead = false;

// are they in the game?
var spectating = false;

// current device ids for iris players
var irisDevices = [];

var cachedNames = [];

// the current help menu page
var currentHelpPage = 0;
var helpDotElements = [];
var helpText = 
[
	"BASICS<br>ABILITIES<br>STATS",
	"HEALTH + DAMAGE<br>WEAPONS<br>LOOT"
];

var NUM_HELP_PAGES = 2;

// array of strings pertaining to item/gun info
var currentItemInfo = [];
var currentGunInfo = [];

// the current items pulled from the opened item cache
var itemCacheInfos = [];

// original skill pts allowed to spend
var originalSkillPts = 0;

// current number of skill pts available for this player
var availableSkillPts = 0;

// original stats of player
var originalStats = [];

// the current upgrade values
var currentUpgradeStats = [];

// the number of battery charges the player has
var availableBatteryCharges = 0;

// the number of available revives
var currentRevives = 0;

// revive counter text
var reviveCounterTextElements = [];

// cooldown UIs
var cooldownContainers = [];

// waiting player text
var waitingPlayerTextElements = [];


// options stuff --------
var supportedQualitySettings = [];

var originalSetting = [];
var currentSetting = [];

var OptionsType =
{
	MUSICVOLUME: 0,
	SFXVOLUME: 1,
	QUALITY: 2
}
var NUM_SETTINGS = 3;

var DifficultyType = 
{
	NORMAL: 0,
	HARD: 1,
	INSANE: 2
}
var difficultyNames = [
	"NORMAL",
	"HARD",
	"INSANE"
];
var NUM_DIFFICULTIES = 3;
var currentDifficulty = 0;

var difficultyImages = [
	"Images/Difficulty_Icons/difficulty_normal.png",
	"Images/Difficulty_Icons/difficulty_hard.png",
	"Images/Difficulty_Icons/difficulty_insane.png"
];

var optionsSettingsPosButtonElements = [];
var optionsSettingsNegButtonElements = [];

var optionsSettingsValueElements = [];
// ----------------------


// click event removing
function removeInlineHandler(element, eventType) 
{
	element.removeAttribute(eventType);
}
function disableClick()
{
	for (var i = 0; i < buttonElements.length; i++)
	{
		removeInlineHandler(buttonElements[i], 'onmousedown');
		removeInlineHandler(buttonElements[i], 'onmouseup');
		removeInlineHandler(buttonElements[i], 'onmouseout');
	}
}

// DEBUG
var debugText;

// menu states
var MenuState =
{
    JOIN: 0,
    CHARACTERSELECT: 1,
    PREGAME: 2,
    ACTION: 3,
    MOVE: 4,
    ATTACK: 5,
    UPGRADE: 6,
    WAITING: 7,
    DEAD: 8,
    GAMEOVER: 9,
    OPTIONS: 10,
    CREDITS: 11,
    SPECTATE: 12,
    UPGRADEWAITING: 13,
    ITEMSELECTION: 14,
    QUITCHECK: 15,
    HELP: 16,
    LEADERBOARDS: 17,
    DISCONNECT: 18,
    SELFONLYITEM: 19,
    LOCATION: 20,
    ABILITY: 21,
    LOADLEVEL: 22,
    ITEM: 23
}
var currentMenuState = MenuState.JOIN;

var optionsReturnMenuState = 0;
var disconnectReturnMenuState = -1;
var helpReturnMenuState = 0;
var leaderboardsReturnMenuState = MenuState.OPTIONS;

var optionsOn = false;

// player stats
var StatType =
{
    HEALTH: 0,
    SPEED: 1,
    RANGE: 2,
    ABILITY: 3,
    GUNPOWER: 4,
    ACTIONS: 5
}
var StatAbbreviations =
[
	"HP",
	"SPD",
	"RNG",
	"ABL",
	"PWR",
	"ACT"
];
var NUM_STATS = 6;

// game over stats
var GameOverStatType =
{
	KILLS: 0,
    FLOORS: 1
}
var NUM_GAME_OVER_STATS = 2;

// colors used for tinting player elements
var playerColor = [
	'#f55e5e',
	'#6ea6d7',
	'#4dbc83',
	'#f5f1a0'
];
var buttonDownGrey = '#969696';

var initCheck = false;

// called when the mobile controller is first loaded
function init()
{
	if (initCheck == true) {return;}
	initCheck = true;

	loadingRoutine = setInterval(loadingAnim, 600);

	// initialize air console
	setupConsole();
	debugText = document.getElementById('debug-content');

	// initialize all menu divs
	initMenuDivs();
	initButtonElements();
	initDPadElements();
	initFooterElements();
	initHeaderElements();
	initCustomHeadElements();
	initHealthBarElements();
	initHealthTextElements();
	initPlayerNameHeaderTextElements();
	initBatteryChargeTextElements();
	initActionTextElements();
	initOptionsButtonElements();
	initSkillPtsTextElements();
	initReviveCounterTextElements();
	initMovesRemainingContainerElements();
	initMovesRemainingTextElements();
	initPlayerSelectButtonElements();
	initPlayerBackoutButtonElements();
	initPlayerDescriptionTextElements();
	initPlayerSelectStatElements();
	initPlayerSelectTriangleElements();
	initPlayerSelectShadowElements();
	initPlayerReadyTriangleElements();
	initPlayerUpgradeTriangleElements();
	initPlayerSelectTextElements();
	initMasterControllerElements();
	initUpgradeStatTextElements();
	initIrisDevices();
	initCurrentUpgradeStats();
	initOriginalStats();
	initGameOverStats();
	initAbilityButtons();
	initCooldownContiners();

	initWaitingPlayerTextElements();
	initHelpButtons();

	// item select
	initItemContainerElements();
	initPlayerItemReadyTriangleElements();
	initItemSelectNameTextElements();
	initItemBackoutNameTextElements();
	initItemSelectShadowElements();
	initItemSelectButtonElements();
	initItemBackoutButtonElements();
	initItemDevices();
	initItemIconImageElements();
	initItemDescriptionTextElements();
	initItemGunStatsContainerElements();
	initItemGunStatSoloContainerElements();
	initGunStatTextElements();
	initItemCacheInfos();

	initHelpInfo();

	// options settings
	initOptionsSettingsPosButtonElements();
	initOptionsSettingsNegButtonElements();
	initOptionsSettingsValueElements();

	// turn off click events if touch device
	if (('ontouchstart' in window) || window.DocumentTouch && document instanceof DocumentTouch)
	{
		disableClick();
	}

	//hideMasterControls();
	equipItem(-1);

	closeStartButton();
	closeAdvanceSessionButton();
	hideCooldownContainers();

	activateMenu(MenuState.JOIN);
}

function initIrisDevices()
{
	irisDevices[0] = null;
	irisDevices[1] = null;
	irisDevices[2] = null;
	irisDevices[3] = null;
}

function setupConsole()
{
	airconsole = new AirConsole({orientation: AirConsole.ORIENTATION_PORTRAIT});
	airconsole.setOrientation(AirConsole.ORIENTATION_PORTRAIT);

	airconsole.onMessage = function (from, data) 
	{
		debugLog("recieving message from: " + from);

	    if (from == 0) 
	    {
	        interperateScreenMessage(data);
	    }
	    else 
	    {
	        interperatePlayerMessage(from, data);
	    }
	}

	airconsole.onConnect = function (device_id)
	{
		cachedNames[device_id] = airconsole.getNickname(device_id);

		debugLog("saving nickname: " + cachedNames[device_id]);
	}
}

var loadingRoutine;
var currentLoadingIndex = 0;

function loadingAnim()
{
	currentLoadingIndex++;
	if (currentLoadingIndex >= loadingImageSequence.length)
	{
		currentLoadingIndex = 0;
	}
	document.getElementById("loadingImage").src = loadingImageSequence[currentLoadingIndex];
}

function stopLoadingAnim()
{
	clearInterval(loadingRoutine);
}

var spectatingRoutine;
var currentSpectatingIndex = 0;
var spectateAnimationActive = false;

function startSpectatingAnim()
{
	if (spectateAnimationActive == true) {return;}

	clearInterval(spectatingRoutine);
	spectatingRoutine = setInterval(spectatingAnim, 600);
	spectateAnimationActive = true;
}

function spectatingAnim()
{
	currentSpectatingIndex++;
	if (currentSpectatingIndex >= spectatingImageSequence.length)
	{
		currentSpectatingIndex = 0;
	}
	document.getElementById("spectatingImage").src = spectatingImageSequence[currentSpectatingIndex];
}

function stopSpectatingAnim()
{
	if (spectateAnimationActive == false) {return;}

	clearInterval(spectatingRoutine);
	spectateAnimationActive = false;
}


// DIV INIT FUNCTIONS START ---------------------------------------

function initMenuDivs()
{
	menuDivs[0] = document.getElementById('joinUI');
	menuDivs[1] = document.getElementById('characterSelectUI');
	menuDivs[2] = document.getElementById('pregameUI');
	menuDivs[3] = document.getElementById('actionUI');
	menuDivs[4] = document.getElementById('directionUI');
	menuDivs[5] = document.getElementById('directionUI');
	menuDivs[6] = document.getElementById('upgradeUI');
	menuDivs[7] = document.getElementById('waitingUI');
	menuDivs[8] = document.getElementById('deadUI');
	menuDivs[9] = document.getElementById('gameoverUI');
	menuDivs[10] = document.getElementById('optionsUI');
	menuDivs[11] = document.getElementById('creditsUI');
	menuDivs[12] = document.getElementById('spectateUI');
	menuDivs[13] = document.getElementById('upgradeWaitingUI');
	menuDivs[14] = document.getElementById('itemSelectionUI');
	menuDivs[15] = document.getElementById('quitCheckUI');
	menuDivs[16] = document.getElementById('helpUI');
	menuDivs[17] = document.getElementById('leaderboardsUI');
	menuDivs[18] = document.getElementById('disconnectUI');
	menuDivs[19] = document.getElementById('directionUI');
	menuDivs[20] = document.getElementById('directionUI');
	menuDivs[21] = document.getElementById('directionUI');
	menuDivs[22] = document.getElementById('loadLevelUI');
	menuDivs[23] = document.getElementById('directionUI');
}

function initButtonElements()
{
	buttonElements = document.getElementsByClassName("buttonElement");
}

function initDPadElements()
{
	dPadElements[0] = document.getElementById('dPad0');
	dPadElements[1] = document.getElementById('dPad1');
	dPadElements[2] = document.getElementById('dPad2');
	dPadElements[3] = document.getElementById('dPad3');
	dPadElements[4] = document.getElementById('dPad4');
	dPadElements[5] = document.getElementById('dPad5');
	dPadElements[6] = document.getElementById('dPad6');
	dPadElements[7] = document.getElementById('dPad7');
	dPadElements[8] = document.getElementById('dPad8');
}

function initFooterElements()
{
	footerElements = document.getElementsByClassName("footerColor");
}

function initHeaderElements()
{
	headerElements = document.getElementsByClassName("elementHeaderCustom");
}

function initCustomHeadElements()
{
	customHeadElements = document.getElementsByClassName("playerHeadCustom");
}

function initHealthBarElements()
{
	healthBarElements = document.getElementsByClassName("healthBar");
}

function initHealthTextElements()
{
	healthTextElements = document.getElementsByClassName("healthText");
}

function initBatteryChargeTextElements()
{
	batteryChargeTextElements = document.getElementsByClassName("batteryChargeText");
}

function initPlayerNameHeaderTextElements()
{
	playerHeaderNameTextElements = document.getElementsByClassName("elementHeaderNameText");
}

function initActionTextElements()
{
	actionTextElements = document.getElementsByClassName("actionsText");
}

function initOptionsButtonElements()
{
	optionsButtonElements = document.getElementsByClassName("optionsButton");
}

function initSkillPtsTextElements()
{
	skillPtsTextElements = document.getElementsByClassName("skillPtsText");
}

function initReviveCounterTextElements()
{
	reviveCounterTextElements = document.getElementsByClassName("reviveCounterText");
}

function initMovesRemainingContainerElements()
{
	movesRemainingContainerElements = document.getElementsByClassName("movesRemaining");
}

function initMovesRemainingTextElements()
{
	movesRemainingTextElements = document.getElementsByClassName("movesRemainingText");
}

function initPlayerSelectButtonElements()
{
	for (var i = 0; i < 4; i++)
	{
		playerSelectButtonElements[i] = document.getElementById("playerSelectButton" + i.toString());
	}
}

function initPlayerBackoutButtonElements()
{
	for (var i = 0; i < 4; i++)
	{
		playerBackoutButtonElements[i] = document.getElementById("playerBackoutButton" + i.toString());
		playerBackoutButtonElements[i].style.display = "none";
	}
}

function initPlayerDescriptionTextElements()
{
	playerDescriptionTextElements[0] = document.getElementById("playerOneDescriptionText");
	playerDescriptionTextElements[1] = document.getElementById("playerTwoDescriptionText");
	playerDescriptionTextElements[2] = document.getElementById("playerThreeDescriptionText");
	playerDescriptionTextElements[3] = document.getElementById("playerFourDescriptionText");
}

function initPlayerSelectStatElements()
{
	for (var i = 0; i < NUM_STATS - 1; i++)
	{
		playerOneSelectStatTextElements[i] = document.getElementById("playerOneSelectStatText" + i.toString());
		playerTwoSelectStatTextElements[i] = document.getElementById("playerTwoSelectStatText" + i.toString());
		playerThreeSelectStatTextElements[i] = document.getElementById("playerThreeSelectStatText" + i.toString());
		playerFourSelectStatTextElements[i] = document.getElementById("playerFourSelectStatText" + i.toString());
	}
}

function initPlayerSelectTriangleElements()
{
	playerSelectInnerTriangleElements[0] = document.getElementById("playerOneInnerTriangle");
	playerSelectInnerTriangleElements[1] = document.getElementById("playerTwoInnerTriangle");
	playerSelectInnerTriangleElements[2] = document.getElementById("playerThreeInnerTriangle");
	playerSelectInnerTriangleElements[3] = document.getElementById("playerFourInnerTriangle");

	playerSelectOuterTriangleElements[0] = document.getElementById("playerOneOuterTriangle");
	playerSelectOuterTriangleElements[1] = document.getElementById("playerTwoOuterTriangle");
	playerSelectOuterTriangleElements[2] = document.getElementById("playerThreeOuterTriangle");
	playerSelectOuterTriangleElements[3] = document.getElementById("playerFourOuterTriangle");

	for (var i = 0; i < playerSelectInnerTriangleElements.length; i++)
	{
		playerSelectInnerTriangleElements[i].style.display = "none";
	}
}

function initPlayerSelectShadowElements()
{
	playerSelectShadowElements[0] = document.getElementById("playerOneSelectShadow");
	playerSelectShadowElements[1] = document.getElementById("playerTwoSelectShadow");
	playerSelectShadowElements[2] = document.getElementById("playerThreeSelectShadow");
	playerSelectShadowElements[3] = document.getElementById("playerFourSelectShadow");

	for (var i = 0; i < playerSelectShadowElements.length; i++)
	{
		playerSelectShadowElements[i].style.display = "none";
	}
}

function initPlayerReadyTriangleElements()
{
	playerReadyTriangleElements[0] = document.getElementById("playerOneReadyTriangle");
	playerReadyTriangleElements[1] = document.getElementById("playerTwoReadyTriangle");
	playerReadyTriangleElements[2] = document.getElementById("playerThreeReadyTriangle");
	playerReadyTriangleElements[3] = document.getElementById("playerFourReadyTriangle");
}

function initPlayerUpgradeTriangleElements()
{
	playerUpgradeTriangleElements[0] = document.getElementById("playerOneUpgradeTriangle");
	playerUpgradeTriangleElements[1] = document.getElementById("playerTwoUpgradeTriangle");
	playerUpgradeTriangleElements[2] = document.getElementById("playerThreeUpgradeTriangle");
	playerUpgradeTriangleElements[3] = document.getElementById("playerFourUpgradeTriangle");
}

function initPlayerSelectTextElements()
{
	for (var i = 0; i < 4; i++)
	{
		playerSelectTextElements[i] = document.getElementById("playerSelectText" + i.toString());
	}
}

function initUpgradeStatTextElements()
{
	for (var i = 0; i < NUM_STATS; i++)
	{
		upgradeStatTextElements[i] = document.getElementById("upgradeStatText" + i.toString());
	}
}

function initMasterControllerElements()
{
	masterControllerElements = document.getElementsByClassName('masterControlElement');
}

function initCurrentUpgradeStats()
{
	for (var i = 0; i < NUM_STATS; i++)
	{
		currentUpgradeStats[i] = 0;
	}
}

function initOriginalStats()
{
	for (var i = 0; i < NUM_STATS; i++)
	{
		originalStats[i] = 0;
	}
}

function initItemContainerElements()
{
	for (var i = 0; i < 5; i++)
	{
		itemContainerElements[i] = document.getElementById("itemContainer" + i.toString());
	}
}

function initItemSelectNameTextElements()
{
	for (var i = 0; i < 5; i++)
	{
		itemSelectNameTextElements[i] = document.getElementById("itemSelectNameText" + i.toString());
	}	
}

function initItemBackoutNameTextElements()
{
	for (var i = 0; i < 4; i++)
	{
		itemBackoutNameTextElements[i] = document.getElementById("itemBackoutText" + i.toString());
	}
}

function initItemSelectShadowElements()
{
	itemSelectShadowElements[0] = document.getElementById("itemOneSelectShadow");
	itemSelectShadowElements[1] = document.getElementById("itemTwoSelectShadow");
	itemSelectShadowElements[2] = document.getElementById("itemThreeSelectShadow");
	itemSelectShadowElements[3] = document.getElementById("itemFourSelectShadow");

	for (var i = 0; i < itemSelectShadowElements.length; i++)
	{
		if (itemSelectShadowElements[i] == null) {continue;}
		itemSelectShadowElements[i].style.display = "none";
	}
}

function initItemSelectButtonElements()
{
	for (var i = 0; i < 5; i++)
	{
		itemSelectButtonElements[i] = document.getElementById("itemSelectButton" + i.toString());
	}
}

function initItemBackoutButtonElements()
{
	for (var i = 0; i < 4; i++)
	{
		itemBackoutButtonElements[i] = document.getElementById("itemBackoutButton" + i.toString());

		if (itemBackoutButtonElements[i] == null) {continue;}
		itemBackoutButtonElements[i].style.display = "none";
	}
}

function initItemDevices()
{
	itemDevices[0] = null;
	itemDevices[1] = null;
	itemDevices[2] = null;
	itemDevices[3] = null;
}

function initPlayerItemReadyTriangleElements()
{
	playerItemReadyTriangleElements[0] = document.getElementById("playerOneItemReadyTriangle");
	playerItemReadyTriangleElements[1] = document.getElementById("playerTwoItemReadyTriangle");
	playerItemReadyTriangleElements[2] = document.getElementById("playerThreeItemReadyTriangle");
	playerItemReadyTriangleElements[3] = document.getElementById("playerFourItemReadyTriangle");
}

function initItemIconImageElements()
{
	for (var i = 0; i < 5; i++)
	{
		itemIconImageElements[i] = document.getElementById("itemIconImage" + i.toString());
	}
}

function initGunStatTextElements()
{
	// four stat jawns
	for (var i = 0; i < 5; i++)
	{
		itemGunStatNameTextElements[i] = [];
		itemGunStatValueTextElements[i] = [];

		for (var k = 0; k < NUM_STATS; k++)
		{
			itemGunStatNameTextElements[i][k] = document.getElementById("item" + i.toString() + "GunStatNameText" + k.toString());
			itemGunStatValueTextElements[i][k] = document.getElementById("item" + i.toString() + "GunStatValueText" + k.toString());
		}
	}
}

function initItemDescriptionTextElements()
{
	for (var i = 0; i < 5; i++)
	{
		itemDescriptionTextElements[i] = document.getElementById("itemDescriptionText" + i.toString());
	}
}

function initItemGunStatsContainerElements()
{
	for (var i = 0; i < 5; i++)
	{
		itemGunStatsContainerElements[i] = document.getElementById("itemGunStatsContainer" + i.toString());
	}
}

function initItemGunStatSoloContainerElements()
{
	for (var i = 0; i < 5; i++)
	{
		itemGunStatSoloContainerElements[i] = [];
		
		for (var k = 0; k < 4; k++)
		{
			itemGunStatSoloContainerElements[i][k] = document.getElementById("item" + i.toString() + "GunStat" + k.toString() + "Container");
		}
	}
}

function initItemCacheInfos()
{
	for (var i = 0; i < 4; i++)
	{
		itemCacheInfos[i] = [];
	}
}

function initGameOverStats()
{
	for (var i = 0; i < NUM_GAME_OVER_STATS; i++)
	{
		gameOverStatValueElements[i] = document.getElementById("gameOverStatValue" + i.toString());
	}
}

function initOptionsSettingsPosButtonElements()
{
	for (var i = 0; i < NUM_SETTINGS; i++)
	{
		optionsSettingsPosButtonElements[i] = document.getElementById("optionsSettingPos" + i.toString());
	}
}
	
function initOptionsSettingsNegButtonElements()
{
	for (var i = 0; i < NUM_SETTINGS; i++)
	{
		optionsSettingsNegButtonElements[i] = document.getElementById("optionsSettingNeg" + i.toString());
	}
}
	
function initOptionsSettingsValueElements()
{
	for (var i = 0; i < NUM_SETTINGS; i++)
	{
		optionsSettingsValueElements[i] = document.getElementById("optionsSettingValue" + i.toString());
	}
}

function initHelpInfo()
{
	for (var i = 0; i < NUM_HELP_PAGES; i++)
	{
		helpDotElements[i] = document.getElementById("helpDot" + i.toString());
	}

	setCurrentHelpPage(0);
}

function initAbilityButtons()
{
	abilityButtonImages = document.getElementsByClassName("abilityButtonImage");
	abilityButtonText = document.getElementsByClassName("abilityButtonText");
}

function initCooldownContiners()
{
	cooldownContainers[0] = document.getElementById("AttackAbilityCooldownContainer");
}

function initWaitingPlayerTextElements()
{
	waitingPlayerTextElements = document.getElementsByClassName("waitingPlayerText");
}

function initHelpButtons()
{
	helpButtonElements = document.getElementsByClassName("helpImage");
}

// DIV INIT FUNCTIONS END -----------------------------------------


// interperates messages from the screen
function interperateScreenMessage(data)
{
	debugLog("screen message: " + data.action);

	if (data.action == undefined) 
	{
		debugLog("ERROR: no action specified from screen");
		return;
	}
	
	if (data.action == 'activateMenu')
	{
		activateMenu(data.menuIndex);
	}
	else if (data.action == "init")
	{
		init();
	}
	else if (data.action == 'turnOnCenterDpad')
	{
		showCenterDirectionButton();
	}
	else if (data.action == 'turnOffCenterDpad')
	{
		hideCenterDirectionButton();
	}
	else if (data.action == 'playerJoinSuccess')
	{
		playerJoinSuccess(data.deviceId, data.irisIndex);
	}
	else if (data.action == 'playerBackoutSuccess')
	{
		playerBackoutSuccess(data.deviceId, data.irisIndex);
	}
	else if (data.action == 'colorUI')
	{
		var colorIndex;
		if (data.playerIndex == -1)
		{
			colorIndex = currentIrisIndex;
		}
		else
		{
			colorIndex = data.playerIndex;
		}

		setBackgroundColor(colorIndex);
		setUIColor(colorIndex);
	}
	else if (data.action == 'allowJoin')
	{
		allowJoin();
	}
	else if (data.action == 'checkMasterController')
	{
		checkMaster();
	}
	else if (data.action == 'characterScreenData')
	{
		updateCharacterJoin(data.slotData);
	}
	else if (data.action == 'upgradeConfirmed')
	{
		if (data.playerIndex == currentIrisIndex)
		{
			activateMenu(MenuState.UPGRADEWAITING);

			clearUpgradeAllocation();
		}
		
		setReadyTriangleColor(playerUpgradeTriangleElements[data.playerIndex], data.playerIndex);
		setUpgradeWaiting(data.readyToStart);
	}
	else if (data.action == 'gameOver')
	{
		onGameOver(data.gameOverStats);
	}
	else if (data.action == 'playerDead')
	{
		dead = true;
		currentRevives = data.playerRevives;
		checkRevives();
		activateMenu(MenuState.DEAD);
	}
	else if (data.action == 'playerRevive')
	{
		dead = false;
		activateMenu(MenuState.ACTION);

		if (currentTurnDevice != airconsole.getDeviceId())
		{
		    setWaitingPlayer(data.currentTurnDevice);
		    activateMenu(MenuState.WAITING);
		}		
	}
	else if (data.action == 'resetPlayer')
	{
		//currentIrisIndex = -1;
		dead = false;
		setBackgroundColor(-1);
		currentItemInfo = [];
		equipItem(-1);
		initItemDevices();
	}
	else if (data.action == 'disconnectActivateUI')
	{
		onDisconnectActivateUI(data.playerIndex, data.disconnectName);
	}
	else if (data.action == 'disconnectDeactivateUI')
	{
		onDisconnectDeactivateUI(data.playerIndex, data.turnPlayerDevice);
	}
	else if (data.action == 'stopSpectating')
	{
		spectating = false;
		disconnectReturnMenuState = -1;
	}
	else if (data.action == 'startSpectating')
	{
		spectating = true;
	}
	else if (data.action == "cooldownStart")
	{
		onCooldownStart(data.abilityId, data.actionCount);
	}
	else if (data.action == "cooldownEnd")
	{
		onCooldownEnd(data.abilityId, data.actionCount);
	}
	else if (data.action == "cooldownDecrement")
	{
		onCooldownDecrement(data.abilityId, data.actionCount);
	}
	else if (data.action == "equipItem")
	{
		equipItem(data.itemType);
	}

	// TODO update sub actions -------------
	else if (data.action == "updateSkillPoints")
	{
		setSkillPtsText(data.skillPoints);
	}
	else if (data.action == 'updateMovesRemaining')
	{
		setMovesRemaining(data.movesRemaining);
	}
	else if (data.action == 'updateActionsRemaining')
	{
		setActions(data.actionsRemaining);
	}
	else if (data.action == 'updateHealth')
	{
		setPlayerHealth(data.currentHealth, data.maxHealth);
	}
	else if (data.action == 'updateStats')
	{
		updateStats(data.stats);
	}
	else if (data.action == "updateBatteryCharge")
	{
		setBatteryCharge(data.numCharges);
	}
	else if (data.action == "updateRevives")
	{
		setRevives(data.numRevives);
	}
	else if (data.action == "updateStory")
	{
		setStoryText(data.storyText);
	}
	else if (data.action == "updateOptions")
	{
		updateOptions(data.qualityNames, data.currentQuality, data.currentMusicVolume, data.currentSFXVolume);
	}

	// TODO session action sub actions -----------
	else if (data.action == 'startGame')
	{
		currentItemInfo = [];
		equipItem(-1);
		initItemDevices();
	}
	else if (data.action == 'advanceTurn')
	{
		onAdvanceTurn(data.previousDevice, data.nextDevice);
	}
	else if (data.action == 'endSession')
	{
		dead = false;
		setPlayerPoseImage(currentIrisIndex);
		activateMenu(MenuState.UPGRADE);

		originalSkillPts = data.availableSkillPts[currentIrisIndex];
		availableSkillPts = originalSkillPts;
		setSkillPtsText(data.availableSkillPts[currentIrisIndex]);
	}

	// TODO item sub actions -------------------------
	else if (data.action == 'itemCacheOpen')
	{
		loadItemCache(data.itemOneInfo, data.itemTwoInfo, data.itemThreeInfo, data.itemFourInfo);

		if (airconsole.getDeviceId() != airconsole.getMasterControllerDeviceId())
		{
			if (data.masterDead)
			{
				document.getElementById("itemConfirmButton").style.display = 'block';
			}
			else
			{
				document.getElementById("itemConfirmButton").style.display = 'none';
			}
		}
	}
	else if (data.action == 'itemSelectSuccess')
	{
		itemSelectSuccess(data.itemIndex, data.playerIndex, data.deviceId);
	}
	else if (data.action == 'itemBackoutSuccess')
	{
		itemBackoutSuccess(data.itemIndex, data.playerIndex);
	}
	else if (data.action == 'itemCacheClose')
	{
		onItemCacheClose(data.turnPlayerDevice);
	}
	else if (data.action == 'updateItemInfo')
	{
		//debugLog("UPDATING ITEM INFO: " + data.newItemInfo.length);
		currentItemInfo = data.newItemInfo;
	}
	else if (data.action == 'updateGunInfo')
	{
		// [Type][Name][Color][Damage][StatType[0]][StatType[2]][StatType[3]...[StatType[Stat.NUM_STATS]]]
		//debugLog("UPDATING GUN INFO: " + data.newGunInfo.length);
		currentGunInfo = data.newGunInfo;

		document.getElementById("attackIcon").src = (gunIcons[currentGunInfo[0]][currentGunInfo[2]]);

		for (var i = 0; i < currentGunInfo.length; i++)
		{
			debugLog(currentGunInfo[i]);
		}
	}

	else
	{
		debugLog("WARNING: unknown action, " + data.action);
	}
}

// interperates messages from another device (not used?)
function interperatePlayerMessage(from, data)
{
	debugLog("player message from " + from.toString() + ": " + data.action.toString());

	if (data.action == 'AdvanceSession')
	{
		activateMenu(MenuState.ACTION);
		for (var i = 0; i < playerUpgradeTriangleElements.length; i++)
		{
			setReadyTriangleColor(playerUpgradeTriangleElements[i], -1);
		}
	}
	else if (data.action == 'StoryContinue')
	{
		if (currentIrisIndex == null || currentIrisIndex == -1)
		{
			spectating = true;
			debugLog("spectating");
			activateMenu(MenuState.SPECTATE);
		}
	}
}

navigator.vibrate = (navigator.vibrate ||
                     navigator.webkitVibrate ||
                     navigator.mozVibrate ||
                     navigator.msVibrate);
function vibrate(vibrate_pattern)
{
	navigator.vibrate(vibrate_pattern);
}

// turns off all menu UI html objects
function deactivateMenus()
{
	for (i = 0; i < menuDivs.length; i++)
	{
		if (menuDivs[i] != null)
		{
			menuDivs[i].style.display = "none";
		}
	}
}

// turns on a specific menu UI from menuIndex
function activateMenu(menuIndex)
{
	if (dead && 
		(menuIndex == MenuState.WAITING
	  || menuIndex == MenuState.ACTION
	  || menuIndex == MenuState.DIRECTION
	  || menuIndex == MenuState.MOVE
	  || menuIndex == MenuState.ATTACK
	  || menuIndex == MenuState.ITEMSELECTION
	  || menuIndex == MenuState.SELFONLYITEM
	  || menuIndex == MenuState.LOCATION
	  || menuIndex == MenuState.ABILITY
	  || menuIndex == MenuState.ITEM)) { return; }

	if (spectating &&
		(menuIndex != MenuState.SPECTATE
	  && menuIndex != MenuState.CHARACTERSELECT
	  && menuIndex != MenuState.DISCONNECT
	  && menuIndex != MenuState.GAMEOVER))
	{
		activateMenu(MenuState.SPECTATE);
		return;
	}

	if (menuIndex == MenuState.PREGAME)
	{
		currentDifficulty = 0;
		setDifficulty(0);
		setWaitingPlayer(airconsole.getMasterControllerDeviceId());
	}

	if (menuIndex == MenuState.CHARACTERSELECT)
	{
		spectating = false;
		resetCooldowns();
		equipItem(-1);
		currentItemInfo = [];

		for (var i = 0; i < 4; i++)
		{
			setItemSelectVizOpen(i)
			setReadyTriangleColor(playerItemReadyTriangleElements[i], -1);
			itemCompareOff(i);
		}
	}

	// if waiting menu, just activate overlay
	if (menuIndex == MenuState.WAITING)
	{
		waitingOn();
		return;
	}

	// if loading menu, just activate overlay
	if (menuIndex == MenuState.LOADLEVEL)
	{
		loadLevelOn();
		return;
	}

	if (menuIndex == MenuState.HELP)
	{
		if (currentMenuState == MenuState.JOIN)
		{
			helpReturnMenuState = currentMenuState;
		}
		else
		{
			helpReturnMenuState = MenuState.OPTIONS;
		}
	}

	if (menuIndex == MenuState.LEADERBOARDS)
	{
		leaderboardsReturnMenuState = currentMenuState;
	}

	if (menuIndex == MenuState.ITEMSELECTION)
	{
		setBackgroundColor(-1);
	}

	if (menuIndex == MenuState.SPECTATE)
	{
		if (currentIrisIndex != null && currentIrisIndex != -1)
		{
			return;
		}
		startSpectatingAnim();
	}
	else if (currentMenuState == MenuState.SPECTATE)
	{
		stopSpectatingAnim();
	}

	if (menuIndex == MenuState.OPTIONS 
	&& currentMenuState != MenuState.CREDITS 
	&& currentMenuState != MenuState.QUITCHECK
	&& currentMenuState != MenuState.LEADERBOARDS
	&& currentMenuState != MenuState.HELP)
	{
		optionsReturnMenuState = currentMenuState;
	}

	if (menuIndex == MenuState.OPTIONS)
	{
		optionsOn = true;
	}
	else
	{
		optionsOn = false;
	}

	if (menuIndex == MenuState.DISCONNECT)
	{
		disconnectReturnMenuState = currentMenuState;
	}

	if (menuDivs[menuIndex] != null)
	{
		deactivateMenus();
		menuDivs[menuIndex].style.display = "block";

		if (menuIndex == MenuState.ATTACK)
		{
			document.getElementById("dPadContainer").style.display = 'block';
			document.getElementById("itemActionContainer").style.display = 'none';
			
			enableCornerDirections();
			hideMovesRemaining();
			document.getElementById("itemContainer4").style.display = 'block';
		}
		else if (menuIndex == MenuState.ABILITY)
		{
			document.getElementById("dPadContainer").style.display = 'block';
			document.getElementById("itemActionContainer").style.display = 'none';
			
			enableCornerDirections();
			hideMovesRemaining();
			document.getElementById("itemContainer4").style.display = 'none';
		}
		else if (menuIndex == MenuState.ITEM)
		{
			document.getElementById("dPadContainer").style.display = 'block';
			document.getElementById("itemActionContainer").style.display = 'none';
			
			enableCornerDirections();
			hideMovesRemaining();
			document.getElementById("itemContainer4").style.display = 'block';
		}
		else if (menuIndex == MenuState.MOVE)
		{
			document.getElementById("dPadContainer").style.display = 'block';
			document.getElementById("itemActionContainer").style.display = 'none';

			disableCornerDirections();
			showMovesRemaining();
			document.getElementById("itemContainer4").style.display = 'none';
		}
		else if (menuIndex == MenuState.LOCATION)
		{
			document.getElementById("dPadContainer").style.display = 'block';
			document.getElementById("itemActionContainer").style.display = 'none';

			disableCornerDirections();
			hideMovesRemaining();
			document.getElementById("itemContainer4").style.display = 'block';
		}
		else if (menuIndex == MenuState.SELFONLYITEM)
		{
			document.getElementById("dPadContainer").style.display = 'none';
			document.getElementById("itemActionContainer").style.display = 'block';

			fillSelfOnlyItemContainer();
			hideMovesRemaining();
			document.getElementById("itemContainer4").style.display = 'block';
		}

		if (menuIndex != MenuState.OPTIONS)
		{
			currentMenuState = menuIndex;
			debugLog("updated current menu state: " + menuIndex.toString());
		}
	}
	else
	{
		debugLog("cannot find menu with index: " + menuIndex);
	}
}

// BUTTONS START --------------------------------------------------

// join
function joinDown()
{
	document.getElementById("joinImage").src = 'Images/join_down.png';
}
function joinUp()
{
	document.getElementById("joinImage").src = 'Images/join.png';
}
function joinGame()
{
	//debugLog("join");
	//playButtonForward();

	joinUp();

	if (airconsole.getDeviceId() == airconsole.getMasterControllerDeviceId())
	{
		showMasterControls();
	}

	var message = 
	{
		'action' : "InitialJoin" 
	};
	airconsole.message(0, message);

	activateMenu(MenuState.CHARACTERSELECT);
}

// credits
function creditsDown()
{
	document.getElementById("creditsButton").style.backgroundColor = buttonDownGrey;
}
function creditsUp()
{
	document.getElementById("creditsButton").style.backgroundColor = 'black';
}
function openCredits()
{
	creditsUp();
	activateMenu(MenuState.CREDITS);
}

// credits back
function creditsBackDown()
{
	document.getElementById("creditsBackButton").style.backgroundColor = buttonDownGrey;
}
function creditsBackUp()
{
	document.getElementById("creditsBackButton").style.backgroundColor = 'black';
}
function creditsBack()
{
	creditsBackUp();
	activateMenu(MenuState.OPTIONS);
}

// options
function optionsDown()
{
	for (var i = 0; i < optionsButtonElements.length; i++)
	{
		optionsButtonElements[i].style.backgroundColor = buttonDownGrey;
	}
}
function optionsUp()
{
	for (var i = 0; i < optionsButtonElements.length; i++)
	{
		optionsButtonElements[i].style.backgroundColor = "black";
	}
}
function optionsOpen()
{
	//debugLog("options");

	optionsUp();

	if (currentMenuState == MenuState.JOIN
	 || currentMenuState == MenuState.CHARACTERSELECT)
	{
		hideOptionsMainMenuButton();
	}
	else
	{
		showOptionsMainMenuButton();
	}

	activateMenu(MenuState.OPTIONS);
}

// dPad
canCenterDpad = false;
function dPadDown(dPadIndex)
{
	if (dPadIndex == 4 && !canCenterDpad) {return;}

	dPadElements[dPadIndex].style.backgroundColor = 'rgba(0,0,0,0.2)';
}
function dPadUp(dPadIndex)
{	
	if (dPadIndex == 4 && !canCenterDpad) {return;}

	dPadElements[dPadIndex].style.backgroundColor = 'rgba(0,0,0,0.1)';
}
function dPad(dPadIndex)
{
	//debugLog(dPadIndex);
	if (dPadIndex == 4 && !canCenterDpad) {return;}

	dPadUp(dPadIndex);

	var message = 
	{
		'action': 'Direction',
		'playerIndex' : currentIrisIndex,
		'directionIndex' : dPadIndex
	};
	airconsole.message(0, message);
}

// cancel
function directionCancelDown()
{
	document.getElementById("directionCancel").style.backgroundColor = 'rgba(0,0,0,0.2)';
}
function directionCancelUp()
{
	document.getElementById("directionCancel").style.backgroundColor = 'rgba(0,0,0,0.1)';
}
function directionCancel()
{
	//debugLog("cancel");

	directionCancelUp();

	var message = 
	{
		'action' : 'PlayerAction',
		'actionType' : 'DirectionCancel',
		'playerIndex' : currentIrisIndex
	};
	airconsole.message(0, message);
}

// confirm
function directionConfirmDown()
{
	document.getElementById("directionConfirm").style.backgroundColor = 'rgba(0,0,0,0.2)';
}
function directionConfirmUp()
{
	document.getElementById("directionConfirm").style.backgroundColor = 'rgba(0,0,0,0.1)';
}
function directionConfirm()
{
	//debugLog("confirm");

	directionConfirmUp();

	var message = 
	{
		'action' : 'PlayerAction',
		'actionType' : 'DirectionConfirm',
		'playerIndex' : currentIrisIndex
	};
	airconsole.message(0, message);
}

// action: move
function actionMoveDown()
{
	document.getElementById("moveButton").style.backgroundColor = 'rgba(0,0,0,0.2)';
}
function actionMoveUp()
{
	document.getElementById("moveButton").style.backgroundColor = 'rgba(0,0,0,0.1)';
}
function actionMove()
{
	//debugLog("action: move");

	actionMoveUp();
	
	var message = 
	{
		'action': 'PlayerAction',
		'actionType' : "Move",
		'playerIndex' : currentIrisIndex
	};
	airconsole.message(0, message);

	showDirectionPlayerHeader();
}

// action: attack
function actionAttackDown()
{
	if (cooldownActions["AttackAbility"] != undefined
	 && cooldownActions["AttackAbility"] > 0) {return;}

	document.getElementById("attackButton").style.backgroundColor = 'rgba(0,0,0,0.2)';
}
function actionAttackUp()
{
	if (cooldownActions["AttackAbility"] != undefined
	 && cooldownActions["AttackAbility"] > 0) {return;}

	document.getElementById("attackButton").style.backgroundColor = 'rgba(0,0,0,0.1)';
}
function actionAttack()
{
	debugLog(cooldownActions["AttackAbility"]);
	if (cooldownActions["AttackAbility"] != undefined
	 && cooldownActions["AttackAbility"] > 0) {return;}
	//debugLog("action: attack");
	actionAttackUp();

	fillItemContainer(4, currentGunInfo);
	
	document.getElementById("itemDescriptionText4").style.display = 'none';
	document.getElementById("itemGunStatsContainer4").style.display = 'block';
	hideDirectionPlayerHeader();

	var message = 
	{
		'action' : 'PlayerAction',
		'actionType' : 'Ability',
		'playerIndex' : currentIrisIndex,
		'abilityType' : 'GunAbility'
	};
	airconsole.message(0, message);
}

// action: item
function actionItemDown()
{
	if (currentItemInfo.length == 0) {return;}

	document.getElementById("itemButton").style.backgroundColor = 'rgba(0,0,0,0.2)';
}
function actionItemUp()
{
	if (currentItemInfo.length == 0) {return;}

	document.getElementById("itemButton").style.backgroundColor = 'rgba(0,0,0,0.1)';
}
function actionItem()
{
	if (currentItemInfo.length == 0) {return;}

	debugLog("action: item");
	for (var i = 0; i < currentItemInfo.length; i++)
	{
		debugLog(currentItemInfo[i].toString());
	}

	actionItemUp();

	fillItemContainer(4, currentItemInfo);

	document.getElementById("itemDescriptionText4").style.display = 'block';
	document.getElementById("itemGunStatsContainer4").style.display = 'none';
	hideDirectionPlayerHeader();
	
	var message = 
	{
		'action' : 'PlayerAction',
		'actionType' : 'Ability',
		'playerIndex' : currentIrisIndex,
		'abilityType' : 'ItemAbility'
	};
	airconsole.message(0, message);
}

// action: skip
function actionSkipDown()
{
	document.getElementById("skipButton").style.backgroundColor = 'rgba(0,0,0,0.2)';
}
function actionSkipUp()
{
	document.getElementById("skipButton").style.backgroundColor = 'rgba(0,0,0,0.1)';
}
function actionSkip()
{
	//debugLog("action: skip");

	actionSkipUp();
	
	var message = 
	{
		'action': 'PlayerAction',
		'actionType' : 'Skip',
		'playerIndex' : currentIrisIndex
	};
	airconsole.message(0, message);
}

// action: DoT
function actionDotDown()
{
	if (availableBatteryCharges <= 0) {return;}

	document.getElementById("actionDot").style.backgroundColor = 'rgba(0,0,0,0.2)';
}
function actionDotUp()
{	
	if (availableBatteryCharges <= 0) {return;}

	document.getElementById("actionDot").style.backgroundColor = 'rgba(0,0,0,0.1)';
}
function actionDot()
{
	if (availableBatteryCharges <= 0) {return;}
	//debugLog("action: DoT");

	actionDotUp();
	
	var message = 
	{
		'action': 'PlayerAction',
		'actionType':'Ability',
		'playerIndex' : currentIrisIndex,
		'abilityType':'DoTAbility'
	};
	airconsole.message(0, message);

	showDirectionPlayerHeader();
}

// action: buff
function actionBuffDown()
{
	if (availableBatteryCharges <= 0) {return;}

	document.getElementById("actionBuff").style.backgroundColor = 'rgba(0,0,0,0.2)';
}
function actionBuffUp()
{
	if (availableBatteryCharges <= 0) {return;}

	document.getElementById("actionBuff").style.backgroundColor = 'rgba(0,0,0,0.1)';
}
function actionBuff()
{
	if (availableBatteryCharges <= 0) {return;}
	//debugLog("action: buff");

	actionBuffUp();
	
	var message = 
	{
		'action': 'PlayerAction',
		'actionType':'Ability',
		'playerIndex' : currentIrisIndex,
		'abilityType' : 'AttackBuffAbility'
	};
	airconsole.message(0, message);

	showDirectionPlayerHeader();
}

// action: heal
function actionHealDown()
{
	if (availableBatteryCharges <= 0) {return;}

	document.getElementById("actionHeal").style.backgroundColor = 'rgba(0,0,0,0.2)';
}
function actionHealUp()
{
	if (availableBatteryCharges <= 0) {return;}

	document.getElementById("actionHeal").style.backgroundColor = 'rgba(0,0,0,0.1)';
}
function actionHeal()
{
	if (availableBatteryCharges <= 0) {return;}
	//debugLog("action: heal");

	actionHealUp();
	
	var message = 
	{
		'action': 'PlayerAction',
		'actionType':'Ability',
		'playerIndex' : currentIrisIndex,
		'abilityType' : 'HealAbility'
	};
	airconsole.message(0, message);

	showDirectionPlayerHeader();
}

// player select
function playerSelectDown(playerIndex)
{
	if (irisDevices[playerIndex] == -1)
	{
		playerSelectButtonElements[playerIndex].style.backgroundColor = buttonDownGrey;
	}
}
function playerSelectUp(playerIndex)
{
	if (irisDevices[playerIndex] == -1)
	{
		playerSelectButtonElements[playerIndex].style.backgroundColor = 'black';
	}
}
function playerSelect(playerIndex)
{
	if (irisDevices[playerIndex] == -1)
	{
		playerSelectUp(playerIndex);

		var message = 
		{
			'action': 'PlayerJoin',
			'playerIndex' : playerIndex
		};
		airconsole.message(0, message);

		//debugLog("playerSelect" + playerIndex);
	}
}

// player backout
function playerBackoutDown(playerIndex)
{
	playerBackoutButtonElements[playerIndex].style.backgroundColor = buttonDownGrey;
}
function playerBackoutUp(playerIndex)
{
	playerBackoutButtonElements[playerIndex].style.backgroundColor = 'black';
}
function playerBackout(playerIndex)
{
	playerBackoutUp(playerIndex);

	//debugLog("sending backout: " + playerIndex);

	var message = 
	{
		'action': 'PlayerBackout',
		'playerIndex' : playerIndex
	};
	airconsole.message(0, message);
}

// start game (character menu)
function startGameDown()
{
	document.getElementById("readyStartButton").style.backgroundColor = buttonDownGrey;
}
function startGameUp()
{
	document.getElementById("readyStartButton").style.backgroundColor = 'black';
}
function startGame()
{
	startGameUp();

	var message = 
	{
		'action': 'StartGame',
	};
	airconsole.message(0, message);
}

// upgrade stat allocation reset
function upgradeResetDown()
{
	document.getElementById("upgradeResetButton").style.backgroundColor = buttonDownGrey;
}
function upgradeResetUp()
{
	document.getElementById("upgradeResetButton").style.backgroundColor = 'black';
}
function upgradeReset()
{
	upgradeResetUp();
	clearUpgradeAllocation();
}

// upgrade stat
function upgradeStatDown(statType)
{
	document.getElementById("upgradeStatButton"+statType.toString()).style.backgroundColor = 'rgba(0,0,0,0.3)';
}
function upgradeStatUp(statType)
{
	document.getElementById("upgradeStatButton"+statType.toString()).style.backgroundColor = 'rgba(0,0,0,0.15)';
}
function upgradeStat(statType)
{
	upgradeStatUp(statType);
	if (availableSkillPts > 0)
	{
		addStatPt(statType);
	}
}

// upgrade continue
function upgradeContinueDown()
{
	document.getElementById("upgradeContinueButton").style.backgroundColor = buttonDownGrey;
}
function upgradeContinueUp()
{
	document.getElementById("upgradeContinueButton").style.backgroundColor = 'black';
}
function upgradeContinue()
{
	upgradeContinueUp();
	confirmUpgrades();
}

// advance session
function advanceSessionDown()
{
	document.getElementById("advanceSessionButton").style.backgroundColor = buttonDownGrey;
}
function advanceSessionUp()
{
	document.getElementById("advanceSessionButton").style.backgroundColor = 'black';
}
function advanceSession()
{
	advanceSessionUp();
	
	var message = 
	{
		'action': 'AdvanceSession',
	};
	airconsole.broadcast(message);

	activateMenu(MenuState.ACTION);
	for (var i = 0; i < playerUpgradeTriangleElements.length; i++)
	{
		setReadyTriangleColor(playerUpgradeTriangleElements[i], -1);
	}

	closeAdvanceSessionButton();
}

// item select
function itemSelectDown(itemIndex)
{
	if (itemDevices[itemIndex] == null)
	{
		itemSelectButtonElements[itemIndex].style.backgroundColor = buttonDownGrey;
	}
}
function itemSelectUp(itemIndex)
{
	if (itemDevices[itemIndex] == null)
	{
		itemSelectButtonElements[itemIndex].style.backgroundColor = 'black';
	}
}
function itemSelect(itemIndex)
{
	if (itemDevices[itemIndex] == null)
	{
		itemSelectUp(itemIndex);

		var message = 
		{
			'action': 'ItemSelect',
			'playerIndex' : currentIrisIndex,
			'itemIndex' : itemIndex
		};
		airconsole.message(0, message);
	}
}

// item backout
function itemBackoutDown(itemIndex)
{
	itemBackoutButtonElements[itemIndex].style.backgroundColor = buttonDownGrey;
}
function itemBackoutUp(itemIndex)
{
	itemBackoutButtonElements[itemIndex].style.backgroundColor = 'black';
}
function itemBackout(itemIndex)
{
	itemBackoutUp(itemIndex);

	//debugLog("sending backout: " + playerIndex);

	var message = 
	{
		'action': 'ItemBackout',
		'playerIndex' : currentIrisIndex,
		'itemIndex' : itemIndex
	};
	airconsole.message(0, message);
}

var topOverlayHeights = [-83, -65.5, -48, -30.5];
var bottomOverlayHeights = [31, 48.5, 66, 83.5];
function itemCompareOn(itemIndex)
{
	document.getElementById("itemCompareButton" + itemIndex.toString()).style.backgroundColor = buttonDownGrey;

	// move and turn on overlay
	//document.getElementById("itemCompareOverlayTop").style.top = topOverlayHeights[itemIndex] + "%";
	//document.getElementById("itemCompareOverlayTop").style.display = "block";
	//document.getElementById("itemCompareOverlayBottom").style.top = bottomOverlayHeights[itemIndex] + "%";
	//document.getElementById("itemCompareOverlayBottom").style.display = "block";

	// switch out item info
	if (itemCacheInfos[itemIndex][0] == ItemType.PISTOL
	 || itemCacheInfos[itemIndex][0] == ItemType.SNIPER
	 || itemCacheInfos[itemIndex][0] == ItemType.SHOTGUN
	 || itemCacheInfos[itemIndex][0] == ItemType.GRENADELAUNCHER)
	{
		fillItemContainer(itemIndex, currentGunInfo);
		itemBackoutNameTextElements[itemIndex].innerHTML = currentGunInfo[1].toUpperCase();
	}
	else
	{
		fillItemContainer(itemIndex, currentItemInfo);
		if (currentItemInfo.length > 0)
		{
			itemBackoutNameTextElements[itemIndex].innerHTML = currentItemInfo[1].toUpperCase();
		}
		else
		{
			itemBackoutNameTextElements[itemIndex].innerHTML = "NONE";
		}
	}
}
function itemCompareOff(itemIndex)
{
	document.getElementById("itemCompareButton" + itemIndex.toString()).style.backgroundColor = 'black';

	// turn off overlay
	//document.getElementById("itemCompareOverlayTop").style.display = "none";
	//document.getElementById("itemCompareOverlayBottom").style.display = "none";

	// switch out item info
	fillItemContainer(itemIndex, itemCacheInfos[itemIndex]);

	itemBackoutNameTextElements[itemIndex].innerHTML = "DROP";
}

function confirmItemsDown()
{
	document.getElementById("itemConfirmButton").style.backgroundColor = buttonDownGrey;
}
function confirmItemsUp()
{
	document.getElementById("itemConfirmButton").style.backgroundColor = 'black';
}
function confirmItems()
{
	confirmItemsUp();

	var message = 
	{
		'action': 'ConfirmItems'
	};

	airconsole.message(0, message);
}

function gameOverContinueDown()
{
	document.getElementById("gameOverContinueButton").style.backgroundColor = buttonDownGrey;
}
function gameOverContinueUp()
{
	document.getElementById("gameOverContinueButton").style.backgroundColor = 'black';
}
function gameOverContinue()
{
	gameOverContinueUp();

	var message = 
	{
		'action': 'GameOverContinue'
	};

	airconsole.message(0, message);
}

function storyContinueDown()
{
	document.getElementById("storyContinueButton").style.backgroundColor = buttonDownGrey;
}
function storyContinueUp()
{
	document.getElementById("storyContinueButton").style.backgroundColor = 'black';
}
function storyContinue()
{
	storyContinueUp();

	var message = 
	{
		'action': 'StoryContinue'
	};

	airconsole.broadcast(message);
}

function reviveDown()
{
	document.getElementById("reviveButton").style.backgroundColor = buttonDownGrey;
}
function reviveUp()
{
	document.getElementById("reviveButton").style.backgroundColor = 'black';
}
function revive()
{
	reviveUp();

	var message = 
	{
		'action': 'Revive',
		'playerIndex': currentIrisIndex
	};

	airconsole.message(0, message);
}

// switch = 1 or -1
function volumeMusicSettingDown(switchValue)
{
	if (switchValue > 0)
	{
		optionsSettingsPosButtonElements[0].style.backgroundColor = 'rgba(0,0,0,0.3)';
	}
	else if (switchValue < 0)
	{
		optionsSettingsNegButtonElements[0].style.backgroundColor = 'rgba(0,0,0,0.3)';
	}
}
function volumeMusicSettingUp(switchValue)
{
	if (switchValue > 0)
	{
		optionsSettingsPosButtonElements[0].style.backgroundColor = 'rgba(0,0,0,0.15)';
	}
	else if (switchValue < 0)
	{
		optionsSettingsNegButtonElements[0].style.backgroundColor = 'rgba(0,0,0,0.15)';
	}
}
function volumeMusicSetting(switchValue)
{
	volumeSettingUp(switchValue);

	if (currentSetting[OptionsType.MUSICVOLUME] + switchValue >= 0 &&
		currentSetting[OptionsType.MUSICVOLUME] + switchValue <= 10)
	{
		currentSetting[OptionsType.MUSICVOLUME] += switchValue;
		setMusicVolume(currentSetting[OptionsType.MUSICVOLUME]);

		var message = 
		{
			'action': 'OptionUpdate',
			'optionIndex': OptionsType.MUSICVOLUME,
			'newValue': currentSetting[OptionsType.MUSICVOLUME]
		};

		airconsole.message(0, message);
	}
}

// switch = 1 or -1
function volumeSFXSettingDown(switchValue)
{
	if (switchValue > 0)
	{
		optionsSettingsPosButtonElements[1].style.backgroundColor = 'rgba(0,0,0,0.3)';
	}
	else if (switchValue < 0)
	{
		optionsSettingsNegButtonElements[1].style.backgroundColor = 'rgba(0,0,0,0.3)';
	}
}
function volumeSFXSettingUp(switchValue)
{
	if (switchValue > 0)
	{
		optionsSettingsPosButtonElements[1].style.backgroundColor = 'rgba(0,0,0,0.15)';
	}
	else if (switchValue < 0)
	{
		optionsSettingsNegButtonElements[1].style.backgroundColor = 'rgba(0,0,0,0.15)';
	}
}
function volumeSFXSetting(switchValue)
{
	volumeSFXSettingUp(switchValue);

	if (currentSetting[OptionsType.SFXVOLUME] + switchValue >= 0 &&
		currentSetting[OptionsType.SFXVOLUME] + switchValue <= 10)
	{
		currentSetting[OptionsType.SFXVOLUME] += switchValue;
		setSFXVolume(currentSetting[OptionsType.SFXVOLUME]);

		var message = 
		{
			'action': 'OptionUpdate',
			'optionIndex': OptionsType.SFXVOLUME,
			'newValue': currentSetting[OptionsType.SFXVOLUME]
		};

		airconsole.message(0, message);
	}
}

// switch = 1 or -1
function qualitySettingDown(switchValue)
{
	if (switchValue > 0)
	{
		optionsSettingsPosButtonElements[2].style.backgroundColor = 'rgba(0,0,0,0.3)';
	}
	else if (switchValue < 0)
	{
		optionsSettingsNegButtonElements[2].style.backgroundColor = 'rgba(0,0,0,0.3)';
	}
}
function qualitySettingUp(switchValue)
{
	if (switchValue > 0)
	{
		optionsSettingsPosButtonElements[2].style.backgroundColor = 'rgba(0,0,0,0.15)';
	}
	else if (switchValue < 0)
	{
		optionsSettingsNegButtonElements[2].style.backgroundColor = 'rgba(0,0,0,0.15)';
	}
}
function qualitySetting(switchValue)
{
	qualitySettingUp(switchValue);

	if (currentSetting[OptionsType.QUALITY] + switchValue >= 0 &&
		currentSetting[OptionsType.QUALITY] + switchValue < supportedQualitySettings.length)
	{
		currentSetting[OptionsType.QUALITY] += switchValue;
		setQualitySetting(currentSetting[OptionsType.QUALITY]);

		var message = 
		{
			'action': 'OptionUpdate',
			'optionIndex': OptionsType.QUALITY,
			'newValue': currentSetting[OptionsType.QUALITY]
		};

		airconsole.message(0, message);
	}
}

// switch = 1 or -1
function difficultySettingDown(switchValue)
{
	if (switchValue > 0)
	{
		document.getElementById("difficultySettingPos").style.backgroundColor = 'rgba(0,0,0,0.3)';
	}
	else if (switchValue < 0)
	{
		document.getElementById("difficultySettingNeg").style.backgroundColor = 'rgba(0,0,0,0.3)';
	}
}
function difficultySettingUp(switchValue)
{
	if (switchValue > 0)
	{
		document.getElementById("difficultySettingPos").style.backgroundColor = 'rgba(0,0,0,0.15)';
	}
	else if (switchValue < 0)
	{
		document.getElementById("difficultySettingNeg").style.backgroundColor = 'rgba(0,0,0,0.15)';
	}
}
function difficultySetting(switchValue)
{
	difficultySettingUp(switchValue);

	if (currentDifficulty + switchValue >= 0 &&
		currentDifficulty + switchValue < NUM_DIFFICULTIES)
	{
		currentDifficulty += switchValue;
		setDifficulty(currentDifficulty);

		var message = 
		{
			'action': 'DifficultyUpdate',
			'newDifficultyIndex': currentDifficulty
		};

		airconsole.message(0, message);
	}
}

function leaderboardsDown()
{
	document.getElementById("leaderboardsButton").style.backgroundColor = buttonDownGrey;
}
function leaderboardsUp()
{
	document.getElementById("leaderboardsButton").style.backgroundColor = 'black';
}
function openLeaderboards()
{
	leaderboardsUp();

	activateMenu(MenuState.LEADERBOARDS);
}

function mainMenuDown()
{
	document.getElementById("mainMenuButton").style.backgroundColor = buttonDownGrey;
}
function mainMenuUp()
{
	document.getElementById("mainMenuButton").style.backgroundColor = 'black';
}
function mainMenu()
{
	mainMenuUp();

	activateMenu(MenuState.QUITCHECK);
}

function optionsResumeDown()
{
	document.getElementById("optionsResumeButton").style.backgroundColor = buttonDownGrey;
}
function optionsResumeUp()
{
	document.getElementById("optionsResumeButton").style.backgroundColor = 'black';
}
function optionsResume()
{
	optionsResumeUp();
	
	optionsOff();
}

function quitCheckYesDown()
{
	document.getElementById("quitCheckYesButton").style.backgroundColor = buttonDownGrey;
}
function quitCheckYesUp()
{
	document.getElementById("quitCheckYesButton").style.backgroundColor = 'black';
}
function quitCheckYes()
{
	quitCheckYesUp();
	
	loadMainMenu();
}

function quitCheckNoDown()
{
	document.getElementById("quitCheckNoButton").style.backgroundColor = buttonDownGrey;
}
function quitCheckNoUp()
{
	document.getElementById("quitCheckNoButton").style.backgroundColor = 'black';
}
function quitCheckNo()
{
	quitCheckNoUp();
	
	activateMenu(MenuState.OPTIONS);
}

function helpDown()
{
	for (var i = 0; i < helpButtonElements.length; i++)
	{
		helpButtonElements[i].src = "Images/help_button_down.png";
	}
}
function helpUp()
{
	for (var i = 0; i < helpButtonElements.length; i++)
	{
		helpButtonElements[i].src = "Images/help_button.png";
	}
}
function helpOpen()
{
	helpUp();

	//debugLog("sending backout: " + playerIndex);

	var message = 
	{
		'action': 'HelpAction',
		'helpAction': "OpenHelp"
	};
	airconsole.message(0, message);

	activateMenu(MenuState.HELP);
}

function upgradeHelpDown()
{
	document.getElementById("upgradeHelpButtonImg").src = "Images/help_button_down.png";

	document.getElementById("upgradeHelp").style.display = 'block';
}

function upgradeHelpUp()
{
	document.getElementById("upgradeHelpButtonImg").src = "Images/help_button.png";

	document.getElementById("upgradeHelp").style.display = 'none';
}

function helpNextDown()
{
	document.getElementById("helpNextButton").style.backgroundColor = buttonDownGrey;
}
function helpNextUp()
{
	document.getElementById("helpNextButton").style.backgroundColor = 'black';
}
function helpNext()
{
	helpNextUp();

	if (currentHelpPage < NUM_HELP_PAGES)
	{
		currentHelpPage++;

		setCurrentHelpPage(currentHelpPage);

		var message = 
		{
			'action': 'HelpAction',
			'helpAction': "PageHelp",
			'pageSwitch': 1
		};
		airconsole.message(0, message);
	}
}

function helpPreviousDown()
{
	document.getElementById("helpPreviousButton").style.backgroundColor = buttonDownGrey;
}
function helpPreviousUp()
{
	document.getElementById("helpPreviousButton").style.backgroundColor = 'black';
}
function helpPrevious()
{
	helpPreviousUp();

	if (currentHelpPage > 0)
	{
		currentHelpPage--;

		setCurrentHelpPage(currentHelpPage);

		var message = 
		{
			'action': 'HelpAction',
			'helpAction': "PageHelp",
			'pageSwitch': -1
		};
		airconsole.message(0, message);
	}
}

function helpBackDown()
{
	document.getElementById("helpBackButton").style.backgroundColor = buttonDownGrey;
}
function helpBackUp()
{
	document.getElementById("helpBackButton").style.backgroundColor = 'black';
}
function helpBack()
{
	helpBackUp();

	activateMenu(helpReturnMenuState);
	setCurrentHelpPage(0);

	var message = 
	{
		'action': 'HelpAction',
		'helpAction': "CloseHelp"
	};
	airconsole.message(0, message);
}

function leaderboardsBackDown()
{
	document.getElementById("leaderboardsBackButton").style.backgroundColor = buttonDownGrey;
}
function leaderboardsBackUp()
{
	document.getElementById("leaderboardsBackButton").style.backgroundColor = 'black';
}
function leaderboardsBack()
{
	leaderboardsBackUp();

	var message = 
	{
		'action': 'LeaderBoardsAction',
		'leaderboardAction': "CloseMenu"
	};
	airconsole.message(0, message);
	
	activateMenu(leaderboardsReturnMenuState);
}

function leaderboardOpenDown(leaderboardIndex)
{
	document.getElementById("leaderboardOpenButton" + leaderboardIndex.toString()).style.backgroundColor = buttonDownGrey;
}
function leaderboardOpenUp(leaderboardIndex)
{
	document.getElementById("leaderboardOpenButton" + leaderboardIndex.toString()).style.backgroundColor = 'black';
}
function leaderboardOpen(leaderboardIndex)
{
	leaderboardOpenUp(leaderboardIndex);
	
	var message = 
	{
		'action': 'LeaderBoardsAction',
		'leaderboardAction': "OpenBoard",
		'boardIndex' : leaderboardIndex
	};
	airconsole.message(0, message);
}

function disconnectContinueDown()
{
	document.getElementById("disconnectContinueButton").style.backgroundColor = buttonDownGrey;
}
function disconnectContinueUp()
{
	document.getElementById("disconnectContinueButton").style.backgroundColor = 'black';
}
function disconnectContinue()
{
	disconnectContinueUp();
	
	var message = 
	{
		'action': 'DisconnectContinue'
	};
	airconsole.broadcast(message);
}

function disconnectFillInDown()
{
	document.getElementById("disconnectFillInButton").style.backgroundColor = buttonDownGrey;
}
function disconnectFillInUp()
{
	document.getElementById("disconnectFillInButton").style.backgroundColor = 'black';
}
function disconnectFillIn()
{
	disconnectFillInUp();
	
	var message = 
	{
		'action': 'DisconnectFillIn'
	};
	airconsole.broadcast(message);
}
// BUTTONS END ----------------------------------------------------


// VIZ SETTING START --------------------------------------------

function setBackgroundColor(index)
{
	if (index == -1)
	{
		document.body.style.backgroundImage = "url('Images/Background/bg_white.png')";
		document.getElementById("upgradeHelpBGImage").src = 'Images/Background/bg_white.png';
	}
	else if (index == 0)
	{
		document.body.style.backgroundImage = "url('Images/Background/bg_red.png')";
		document.getElementById("upgradeHelpBGImage").src = 'Images/Background/bg_red.png';
	}
	else if (index == 1)
	{
		document.body.style.backgroundImage = "url('Images/Background/bg_blue.png')";
		document.getElementById("upgradeHelpBGImage").src = 'Images/Background/bg_blue.png';
	}
	else if (index == 2)
	{
		document.body.style.backgroundImage = "url('Images/Background/bg_green.png')";
		document.getElementById("upgradeHelpBGImage").src = 'Images/Background/bg_green.png';
	}
	else if (index == 3)
	{
		document.body.style.backgroundImage = "url('Images/Background/bg_yellow.png')";
		document.getElementById("upgradeHelpBGImage").src = 'Images/Background/bg_yellow.png';
	}
}

var imageURL = undefined;
function setPlayerProfilePic(element)
{
	if (imageURL == undefined)
	{
		imageURL = airconsole.getProfilePicture();

		if (imageURL.includes("https"))
		{
			imageURL = imageURL.replace("s", "");
		}
	}

	if (imageURL == undefined)
	{
		element.src = "Images/AC_default_user.png";
	}
	else
	{
		imageExists(imageURL, function(exists)
		{
			if (exists == true)
			{
				element.src = imageURL;
			}
			else
			{
				element.src = "Images/AC_default_user.png";
			}
		});
	}

	debugLog(imageURL);
}

function imageExists(url, callback) 
{
  var img = new Image();
  img.onload = function() { callback(true); };
  img.onerror = function() { callback(false); };
  img.src = url;
}

function setInnerTriangleColor(element, index)
{
	if (index < 0 || index > 3)
	{
		element.src = "Images/Player_Select/player_select_inner_triangle_black.png";
	}
	else
	{
		element.src = "Images/Player_Select/player_select_inner_triangle_"+index.toString()+".png";
	}
}

function setReadyTriangleColor(element, index)
{
	if (index < 0 || index > 3)
	{
		element.src = "Images/Player_Select/player_select_ready_triangle_black.png";
	}
	else
	{
		element.src = "Images/Player_Select/player_select_ready_triangle_"+index.toString()+".png";
	}
}

function setUIColor(index)
{
	if (index == -1) {return;}

	for(var i=0; i<customHeadElements.length; i++) 
	{
	    setPlayerProfilePic(customHeadElements[i]);
	}
	
	for(var i=0; i<headerElements.length; i++) 
	{
	    headerElements[i].style.backgroundColor = playerColor[index];
	}

	for(var i=0; i<footerElements.length; i++) 
	{
	    footerElements[i].style.backgroundColor = playerColor[index];
	}
}

function setPlayerPoseImage(playerIndex)
{
	if (playerIndex > 0 && playerIndex < 4)
	{
		document.getElementById("playerPoseImg").src = "Images/Player_Poses/player_pose_" + playerIndex.toString() + ".png";
	}
}

function hideMasterControls()
{
	for (var i = 0; i < masterControllerElements.length; i++)
	{
		masterControllerElements[i].style.display = 'none';
	}

	document.getElementById("pregameMasterScreen").style.display = "none";
	document.getElementById("pregameSlaveScreen").style.display = "block";
}

function showMasterControls()
{
	for (var i = 0; i < masterControllerElements.length; i++)
	{
		masterControllerElements[i].style.display = 'block';
	}

	document.getElementById("pregameMasterScreen").style.display = "block";
	document.getElementById("pregameSlaveScreen").style.display = "none";
}

function disableCornerDirections()
{
	dPadElements[0].style.display = "none";
	dPadElements[2].style.display = "none";
	dPadElements[6].style.display = "none";
	dPadElements[8].style.display = "none";
}

function enableCornerDirections()
{
	dPadElements[0].style.display = "block";
	dPadElements[2].style.display = "block";
	dPadElements[6].style.display = "block";
	dPadElements[8].style.display = "block";
}

function setUpgradeReadyImage(ready)
{
	if (ready == true)
	{
		document.getElementById("upgradeReadyImage").src = "Images/upgrade_ready.png";
	}
	else
	{
		document.getElementById("upgradeReadyImage").src = "Images/upgrade_wait.png";
	}
}

function setCharacterSelectReadyImage(ready)
{
	if (ready == true)
	{
		document.getElementById("characterSelectReadyImage").style.display = 'block';
	}
	else
	{
		document.getElementById("characterSelectReadyImage").style.display = 'none';
	}
}

function hideCooldownContainers()
{
	for (var i = 0; i < cooldownContainers.length; i++)
	{
		cooldownContainers[i].style.display = "none";
	}
}

function showOptionsMainMenuButton()
{
	document.getElementById("mainMenuButton").style.display = 'block';
}

function hideOptionsMainMenuButton()
{
	document.getElementById("mainMenuButton").style.display = 'none';
}

function showCenterDirectionButton()
{
	canCenterDpad = true;
	document.getElementById("dPad4").style.backgroundColor = 'rgba(0,0,0,0.1)';
}

function hideCenterDirectionButton()
{
	canCenterDpad = false;
	document.getElementById("dPad4").style.backgroundColor = 'rgba(0,0,0,0)';
}

function hideDirectionPlayerHeader()
{
	document.getElementById("directionPlayerHeader").style.display = 'none';
	document.getElementById("directionPlayerHead").style.display = 'none';
	document.getElementById("directionHealthBar").style.display = 'none';
}

function showDirectionPlayerHeader()
{
	document.getElementById("directionPlayerHeader").style.display = 'block';
	document.getElementById("directionPlayerHead").style.display = 'block';
	document.getElementById("directionHealthBar").style.display = 'block';
}

// VIZ SETTING END ----------------------------------------------


// DATA SETTING FUNCTIONS START -----------------------------------

function setPlayerHealth(currentValue, maxValue)
{
	for(var i=0; i<healthBarElements.length; i++) 
	{
	    healthBarElements[i].style.width = ((currentValue / maxValue) * 100) + "%";
	}

	for(var i=0; i<healthTextElements.length; i++) 
	{
	    healthTextElements[i].innerHTML = currentValue.toString();
	}
}

function setActions(numActions)
{
	var newActionsText;
	if (numActions < 10)
	{
		newActionsText = "0" + numActions.toString();
	}
	else
	{
		newActionsText = numActions.toString();
	}

	for(var i=0; i<actionTextElements.length; i++) 
	{
	    actionTextElements[i].innerHTML = newActionsText;
	}
}

function setSkillPtsText(numPts)
{
	var newPtsText;
	if (numPts < 10)
	{
		newPtsText = "0" + numPts.toString();
	}
	else
	{
		newPtsText = numPts.toString();
	}

	for(var i=0; i<skillPtsTextElements.length; i++) 
	{
	    skillPtsTextElements[i].innerHTML = newPtsText;
	}
}

function setMovesRemaining(numMoves)
{
	var newMovesText;
	if (numMoves < 10)
	{
		newMovesText = "0" + numMoves.toString();
	}
	else
	{
		newMovesText = numMoves.toString();
	}

	for(var i=0; i<movesRemainingTextElements.length; i++) 
	{
	    movesRemainingTextElements[i].innerHTML = newMovesText;
	}
}

function setPlayerJoinVizOpen(playerIndex)
{
	playerSelectButtonElements[playerIndex].style.display = "block";
	playerSelectButtonElements[playerIndex].style.backgroundColor = 'black';
	playerSelectTextElements[playerIndex].innerHTML = "SELECT";
	playerSelectTextElements[playerIndex].style.color = 'white';

	playerBackoutButtonElements[playerIndex].style.display = "none";

	playerSelectShadowElements[playerIndex].style.display = "none";

	playerSelectInnerTriangleElements[playerIndex].style.display = "none";
	playerSelectOuterTriangleElements[playerIndex].style.display = "block";

	setReadyTriangleColor(playerReadyTriangleElements[playerIndex], -1);
}

function setPlayerJoinVizThis(playerIndex)
{
	playerSelectButtonElements[playerIndex].style.display = "none";
	playerSelectButtonElements[playerIndex].style.backgroundColor = 'black';

	playerBackoutButtonElements[playerIndex].style.display = "block";

	playerSelectShadowElements[playerIndex].style.display = "block";

	playerSelectInnerTriangleElements[playerIndex].style.display = "block";
	setInnerTriangleColor(playerSelectInnerTriangleElements[playerIndex], -1);
	playerSelectOuterTriangleElements[playerIndex].style.display = "block";

	setReadyTriangleColor(playerReadyTriangleElements[playerIndex], playerIndex);
}

function setPlayerJoinVizOther(playerIndex, deviceId)
{
	playerSelectButtonElements[playerIndex].style.display = "block";
	playerSelectButtonElements[playerIndex].style.backgroundColor = playerColor[playerIndex];
	playerSelectTextElements[playerIndex].innerHTML = airconsole.getNickname(deviceId).toUpperCase();
	playerSelectTextElements[playerIndex].style.color = 'black';

	playerBackoutButtonElements[playerIndex].style.display = "none";

	playerSelectShadowElements[playerIndex].style.display = "none";

	playerSelectInnerTriangleElements[playerIndex].style.display = "block";
	setInnerTriangleColor(playerSelectInnerTriangleElements[playerIndex], playerIndex);
	playerSelectOuterTriangleElements[playerIndex].style.display = "none";
	
	setReadyTriangleColor(playerReadyTriangleElements[playerIndex], playerIndex);
}

function setPlayerName(playerName)
{
	for (var i = 0; i < playerHeaderNameTextElements.length; i++)
	{
		playerHeaderNameTextElements[i].innerHTML = playerName.toString().toUpperCase();
	}
}

function setBatteryCharge(numCharges)
{
	var chargeText;
	if (numCharges < 10)
	{
		chargeText = "0" + numCharges.toString();
	}
	else
	{
		chargeText = numCharges.toString();
	}

	for (var i = 0; i < batteryChargeTextElements.length; i++)
	{
		batteryChargeTextElements[i].innerHTML = chargeText;
	}

	availableBatteryCharges = numCharges;

	if (availableBatteryCharges <= 0)
	{
		setBatteryAbilityButtonOpacity(0.3);
	}
	else
	{
		setBatteryAbilityButtonOpacity(1);
	}
}

function setBatteryAbilityButtonOpacity(newOpacity)
{
	for (var i = 0; i < abilityButtonImages.length; i++)
	{
		abilityButtonImages[i].style.opacity = newOpacity;
		abilityButtonText[i].style.opacity = newOpacity;
	}
}

function setAbilityButtonOpacity(abilityId, newOpacity)
{
	if (abilityId == "AttackAbility")
	{
		document.getElementById("attackIcon").style.opacity = newOpacity;
		if (newOpacity == 0)
		{
			document.getElementById("actionText1").innerHTML = "COOLDOWN";
		}
		else 
		{
			document.getElementById("actionText1").innerHTML = "ATTACK";
		}
	}
	else
	{
		document.getElementById(abilityId + "Icon").style.opacity = newOpacity;
		document.getElementById(abilityId + "Text").style.opacity = newOpacity;
	}
}

function setUpgradeWaiting(readyToStart)
{
	// master
	if (airconsole.getDeviceId() == airconsole.getMasterControllerDeviceId())
	{
		if (readyToStart == true)
		{
			document.getElementById("upgradeWaitingHeaderText").innerHTML = "START<br>NEXT<br>FLOOR";
			openAdvanceSessionButton();
		}
		else 
		{
			document.getElementById("upgradeWaitingHeaderText").innerHTML = "WAITING<br>ON<br>OTHER<br>PLAYERS";
		}
	}

	// slave ;)
	else
	{
		if (readyToStart == true)
		{
			document.getElementById("upgradeWaitingHeaderText").innerHTML = "WAITING<br>ON<br>" + airconsole.getNickname(airconsole.getMasterControllerDeviceId()).toUpperCase();
		}
		else 
		{
			document.getElementById("upgradeWaitingHeaderText").innerHTML = "WAITING<br>ON<br>OTHER<br>PLAYERS";
		}
	}

	setUpgradeReadyImage(readyToStart);
}

function equipItem(itemType)
{
	// unequipped item
	if (itemType == -1)
	{
		document.getElementById("itemIcon").src = "Images/Item_Icons/item_none.png";
		document.getElementById("itemIcon").style.opacity = 0.3;

		document.getElementById("actionText2").innerHTML = "NO ITEM";
		document.getElementById("actionText2").style.opacity = 0.3;
	}
	// new weapon end old cooldown
	else if (itemType == ItemType.PISTOL
	 	  || itemType == ItemType.SNIPER
		  || itemType == ItemType.SHOTGUN
		  || itemType == ItemType.GRENADELAUNCHER)
	{
		onCooldownEnd("AttackAbility", 0);
	}
	// non-pickup item, show item
	else if (itemType != ItemType.REVIVE
		  && itemType != ItemType.STATPOINT
		  && itemType != ItemType.BATTERY)
	{
		document.getElementById("itemIcon").src = itemIcons[itemType];
		document.getElementById("itemIcon").style.opacity = 1;

		document.getElementById("actionText2").innerHTML = itemNames[itemType];
		document.getElementById("actionText2").style.opacity = 1;
	}
}

function setItemIconImage(itemIndex, itemType)
{
	if (itemType == -1)
	{
		itemIconImageElements[itemIndex].style.display = 'none';
		return;
	}

	itemIconImageElements[itemIndex].style.display = 'block';
	itemIconImageElements[itemIndex].src = itemIcons[itemType];
}

function setGunIconImage(itemIndex, gunType, colorIndex)
{
	itemIconImageElements[itemIndex].style.display = 'block';
	itemIconImageElements[itemIndex].src = gunIcons[gunType][colorIndex];
}

function setItemName(itemIndex, itemName)
{
	itemSelectNameTextElements[itemIndex].innerHTML = itemName.toUpperCase();
}

function setItemSelectShadowColor(itemIndex, colorIndex)
{
	itemSelectShadowElements[itemIndex].style.backgroundColor = playerColor[colorIndex];
}

// takes the itemIndex and the itemInfo string to parse
function fillItemContainer(itemIndex, itemInfo)
{
	// get item type
	//debugLog(itemInfo.length);

	if (itemInfo.length == 0)
	{
		// fill w/ empty
		setItemName(itemIndex, "NONE");

		hideItemDescription(itemIndex);
		hideGunStats(itemIndex);

		setItemIconImage(itemIndex, -1);
		itemDescriptionTextElements[itemIndex].innerHTML = "";

		return;
	}

	var itemTypeIndex = itemInfo[0];
	if (itemTypeIndex == undefined || itemTypeIndex < 0 || itemTypeIndex >= NUM_ITEMS)
	{
		debugLog("unknown item for slot " + itemIndex + ": " + itemInfo[0]);
		return;
	}
	else
	{
		// name
		if (itemInfo[1] == undefined)
		{
			setItemName(itemIndex, itemNames[itemTypeIndex]);
		}
		else
		{
			setItemName(itemIndex, itemInfo[1]);
		}

		if (itemTypeIndex == ItemType.PISTOL
		 || itemTypeIndex == ItemType.SNIPER
		 || itemTypeIndex == ItemType.SHOTGUN
		 || itemTypeIndex == ItemType.GRENADELAUNCHER)
		{
			showGunStats(itemIndex);
			hideItemDescription(itemIndex);

			setGunIconImage(itemIndex, itemTypeIndex, itemInfo[2]);
			fillGunStats(itemIndex, itemInfo);
		}
		else
		{
			showItemDescription(itemIndex);
			hideGunStats(itemIndex);

			setItemIconImage(itemIndex, itemTypeIndex);
			itemDescriptionTextElements[itemIndex].innerHTML = itemInfo[2];
		}

		if (itemTypeIndex == ItemType.REVIVE
		 || itemTypeIndex == ItemType.BATTERY
		 || itemTypeIndex == ItemType.STATPOINT)
		{
			document.getElementById("itemCompareButton" + itemIndex.toString()).style.display = 'none';
		}
		else if (itemIndex != 4)
		{
			document.getElementById("itemCompareButton" + itemIndex.toString()).style.display = 'block';
		}

		showItemContainer(itemIndex);
	}
}

function showItemContainer(itemIndex)
{
	itemContainerElements[itemIndex].style.display = "block";
}

function hideItemContainer(itemIndex)
{
	itemContainerElements[itemIndex].style.display = "none";
}

function showItemDescription(itemIndex)
{
	itemDescriptionTextElements[itemIndex].style.display = "block";
}

function hideItemDescription(itemIndex)
{
	itemDescriptionTextElements[itemIndex].style.display = "none";
}

function fillGunStats(itemIndex, itemInfo)
{
	var numBuffed = 0;
	for (var i = 0; i < NUM_STATS; i++)
	{
		if (numBuffed < 3 && itemInfo[4 + i] > 0)
		{
			// found buffed stat
			fillGunStat(itemIndex, numBuffed, StatAbbreviations[i], itemInfo[4 + i]);
			showGunStat(itemIndex, numBuffed);
			numBuffed++;
		}
	}

	for (var i = 2; i >= numBuffed; i--)
	{
		hideGunStat(itemIndex, i);
	}

	// damage
	fillGunStat(itemIndex, 3, "DMG", itemInfo[3]);
}

function showGunStat(itemIndex, statIndex)
{
	itemGunStatSoloContainerElements[itemIndex][statIndex].style.display = 'block';
}

function hideGunStat(itemIndex, statIndex)
{
	itemGunStatSoloContainerElements[itemIndex][statIndex].style.display = 'none';
}

function fillGunStat(itemIndex, statIndex, statName, statValue)
{
	var plusJawn = "";
	if (statIndex < 3)
	{
		plusJawn = "+";
	}

	itemGunStatNameTextElements[itemIndex][statIndex].innerHTML = statName;
	itemGunStatValueTextElements[itemIndex][statIndex].innerHTML = plusJawn + statValue.toString();
}

function setItemSelectVizThis(itemIndex)
{
	setItemSelectShadowColor(itemIndex, currentIrisIndex);
	itemSelectShadowElements[itemIndex].style.display = 'block';
	itemSelectButtonElements[itemIndex].style.display = 'none';
	itemBackoutButtonElements[itemIndex].style.display = 'block';
	itemSelectNameTextElements[itemIndex].style.color = "white";
}

function setItemSelectVizOther(itemIndex, playerIndex)
{
	itemSelectShadowElements[itemIndex].style.display = 'none';
	itemSelectButtonElements[itemIndex].style.backgroundColor = playerColor[playerIndex];

	itemSelectNameTextElements[itemIndex].style.color = "black";
}

function setItemSelectVizOpen(itemIndex)
{
	itemSelectShadowElements[itemIndex].style.display = 'none';
	itemSelectButtonElements[itemIndex].style.backgroundColor = 'black';
	itemSelectButtonElements[itemIndex].style.display = 'block';
	itemBackoutButtonElements[itemIndex].style.display = 'none';
	itemSelectNameTextElements[itemIndex].style.color = "white";
}

function showGunStats(itemIndex)
{
	itemGunStatsContainerElements[itemIndex].style.display = "block";
}

function hideGunStats(itemIndex)
{
	itemGunStatsContainerElements[itemIndex].style.display = "none";
}

function setGameOverStat(statIndex, statValue)
{
	gameOverStatValueElements[statIndex].innerHTML = statValue.toString();
}

function setStoryText(storyText)
{
	document.getElementById("storyText").innerHTML = storyText;
}

function setRevives(numRevives)
{
	if (numRevives < 0) {numRevives = 0;}

	currentRevives = numRevives;

	var newRevives = "";
	if (numRevives < 10)
	{
		newRevives = "0";
	}
	newRevives += numRevives.toString();

	for (var i = 0; i < reviveCounterTextElements.length; i++)
	{
		reviveCounterTextElements[i].innerHTML = newRevives;
	}
}

function setQualitySetting(newQuality)
{
	currentSetting[OptionsType.QUALITY] = newQuality;

	optionsSettingsValueElements[OptionsType.QUALITY].innerHTML = supportedQualitySettings[newQuality];
}

function setMusicVolume(newVolume)
{
	currentSetting[OptionsType.MUSICVOLUME] = newVolume;

	var volumeText = "";
	if (newVolume < 10)
	{
		volumeText = "0";
	}
	volumeText += newVolume.toString();

	optionsSettingsValueElements[OptionsType.MUSICVOLUME].innerHTML = volumeText;
}

function setSFXVolume(newSFXVolume)
{
	currentSetting[OptionsType.SFXVOLUME] = newSFXVolume;

	var sfxVolumeText = "";
	if (newSFXVolume < 10)
	{
		sfxVolumeText = "0";
	}
	sfxVolumeText += newSFXVolume.toString();

	optionsSettingsValueElements[OptionsType.SFXVOLUME].innerHTML = sfxVolumeText;
}

function setDifficulty(newDifficultyIndex)
{
	document.getElementById("difficultyImage").src = difficultyImages[newDifficultyIndex];
	document.getElementById("difficultySettingText").innerHTML = difficultyNames[newDifficultyIndex];

	if (newDifficultyIndex == 0)
	{
		document.getElementById("difficultyFloorDescriptionText").innerHTML = "START ON THE <b>1ST FLOOR</b>";
	}
	else if (newDifficultyIndex == 1)
	{
		document.getElementById("difficultyFloorDescriptionText").innerHTML = "START ON THE <b>3RD FLOOR</b>";
	}
	else if (newDifficultyIndex == 2)
	{
		document.getElementById("difficultyFloorDescriptionText").innerHTML = "START ON THE <b>5TH FLOOR</b>";
	}
}

function setCurrentHelpPage(newPage)
{
	for (var i = 0; i < NUM_HELP_PAGES; i++)
	{
		if (i == newPage)
		{
			helpDotElements[i].src = "Images/help_dot_closed.png";
		}
		else
		{
			helpDotElements[i].src = "Images/help_dot_open.png";
		}
	}

	if (newPage <= 0)
	{
		document.getElementById("helpPreviousButton").style.display = "none";
	}
	else 
	{
		document.getElementById("helpPreviousButton").style.display = "block";
	}

	if (newPage >= NUM_HELP_PAGES - 1)
	{
		document.getElementById("helpNextButton").style.display = "none";
	}
	else 
	{
		document.getElementById("helpNextButton").style.display = "block";
	}

	document.getElementById("helpText").innerHTML = helpText[newPage];
}

function fillSelfOnlyItemContainer()
{
	document.getElementById("selfOnlyItemIcon").src = itemIcons[currentItemInfo[0]];
	document.getElementById("selfOnlyItemText").innerHTML = "ACTIVATE<br>" + currentItemInfo[1].toUpperCase() + "?";
}

function GetCachedName(waitingPlayerDevice)
{
	return cachedNames[waitingPlayerDevice];
}

// DATA SETTING FUNCTIONS END -------------------------------------

function onCooldownStart(abilityId, actionCount)
{
	if (actionCount > 0)
	{
		if (abilityId == "PistolAbility" 
		 || abilityId == "SniperAbility"
		 || abilityId == "ShotgunAbility"
		 || abilityId == "GrenadeLauncherAbility")
		{
			abilityId = "AttackAbility";
		}

		document.getElementById(abilityId + "CooldownContainer").style.display = 'block';
		document.getElementById(abilityId + "CooldownText").innerHTML = actionCount.toString();

		cooldownActions[abilityId] = actionCount;

		var count = 0;
		for (a in cooldownKeys) 
		{
		    if (cooldownKeys.hasOwnProperty(a)) 
		    {
		        count++;
		    }
	    }

		var found = false;
		for (var i = 0; i < count; i++)
		{
			if (cooldownKeys[i] == abilityId)
			{
				found = true;
			}
		}

		if (found == false)
		{
			debugLog("adding new cooldown key: " + abilityId.toString() + ", cooldownLength = " + count.toString());
			cooldownKeys[count] = abilityId;
		}

		if (abilityId == "AttackAbility")
		{
			setAbilityButtonOpacity(abilityId, 0);
		}
		else
		{
			setAbilityButtonOpacity(abilityId, 0.3);
		}
	}
}

function onCooldownEnd(abilityId, actionCount)
{
	if (abilityId == "PistolAbility" 
	 || abilityId == "SniperAbility"
	 || abilityId == "ShotgunAbility"
	 || abilityId == "GrenadeLauncherAbility")
	{
		abilityId = "AttackAbility";
	}

	document.getElementById(abilityId + "CooldownContainer").style.display = 'none';

	cooldownActions[abilityId] = 0;

	setAbilityButtonOpacity(abilityId, 1);
}

function onCooldownDecrement(abilityId, actionCount)
{
	if (abilityId == "PistolAbility" 
	 || abilityId == "SniperAbility"
	 || abilityId == "ShotgunAbility"
	 || abilityId == "GrenadeLauncherAbility")
	{
		abilityId = "AttackAbility";
	}

	cooldownActions[abilityId] = actionCount;

	document.getElementById(abilityId + "CooldownText").innerHTML = cooldownActions[abilityId];
}

function resetCooldowns()
{
	var count = 0;
	for (a in cooldownKeys) 
	{
	    if (cooldownKeys.hasOwnProperty(a)) 
	    {
	        count++;
	    }
    }

	for (var i = 0; i < count; i++)
	{
		debugLog(i);
		debugLog(cooldownKeys[i]);
		
		onCooldownEnd(cooldownKeys[i], 0);
	}
}

function checkMaster()
{
	if (airconsole.getDeviceId() == airconsole.getMasterControllerDeviceId())
	{
		showMasterControls();
		queryOptions();

		setDisconnectUI(true);
	}
	else
	{
		hideMasterControls();

		setDisconnectUI(false);
	}
}

function queryOptions()
{
	var message = 
	{
		'action': 'QueryOptions'
	};

	airconsole.message(0, message);
}

function updateOptions(newQualityNames, newQuality, newMusicVolume, newSFXVolume)
{	
	supportedQualitySettings = newQualityNames;
	for (var i = 0; i < supportedQualitySettings.length; i++)
	{
		supportedQualitySettings[i] = supportedQualitySettings[i].toUpperCase();
	}

	setQualitySetting(newQuality);

	setMusicVolume(newMusicVolume);

	setSFXVolume(newSFXVolume);
}

function checkRevives()
{
	if (currentRevives <= 0)
	{
		document.getElementById("reviveButton").style.display = 'none';
		document.getElementById("reviveOrText").style.display = 'none';
		document.getElementById("reviveNoneText").style.display = 'block';
	}
	else
	{
		document.getElementById("reviveButton").style.display = 'block';
		document.getElementById("reviveOrText").style.display = 'block';
		document.getElementById("reviveNoneText").style.display = 'none';
	}
}

function playerJoinSuccess(deviceId, irisIndex)
{
	if (deviceId == airconsole.getDeviceId())
	{
		// this device became that iris player
		setPlayerJoinVizThis(irisIndex);
		currentIrisIndex = irisIndex;
		setPlayerName(airconsole.getNickname(deviceId));
	}
	else
	{
		// another device became that iris player
		setPlayerJoinVizOther(irisIndex, deviceId);
	}

	irisDevices[irisIndex] = deviceId;

	if (deviceId == airconsole.getMasterControllerDeviceId())
	{
		openStartButton();
	}

	setCharacterSelectReadyImage(true);
}

function playerBackoutSuccess(itemIndex, playerIndex, deviceId)
{
	if (deviceId == airconsole.getDeviceId()
	 || playerIndex == currentIrisIndex)
	{
		currentIrisIndex = -1;
	}

	setPlayerJoinVizOpen(playerIndex);
	irisDevices[playerIndex] = -1;

	for (var i = 0; i < irisDevices.length; i++)
	{
		if (irisDevices[i] != -1)
		{
			return;
		}
	}

	closeStartButton();
	setCharacterSelectReadyImage(false);
}

function itemSelectSuccess(itemIndex, playerIndex, deviceId)
{
	if (deviceId == airconsole.getDeviceId())
	{
		setItemSelectVizThis(itemIndex);
	}
	else
	{
		setItemSelectVizOther(itemIndex, playerIndex, deviceId);
	}

	itemDevices[itemIndex] = deviceId;
	setReadyTriangleColor(playerItemReadyTriangleElements[playerIndex], playerIndex);
}

function itemBackoutSuccess(itemIndex, playerIndex)
{
	setItemSelectVizOpen(itemIndex);
	itemDevices[itemIndex] = null;
	setReadyTriangleColor(playerItemReadyTriangleElements[playerIndex], -1);
}

function onAdvanceTurn(previousDevice, nextDevice)
{
	//debugLog("onAdvanceTurn: " + previousDevice + " " + nextDevice);
    if (nextDevice == airconsole.getDeviceId())
	{
		turnStart();
	}
	else if (previousDevice == airconsole.getDeviceId())
	{
		turnEnd();
	}
	else if (spectating == false)
	{
		if (optionsOn == false)
		{
			activateMenu(MenuState.WAITING);
		}
		else
		{
			optionsReturnMenuState = MenuState.WAITING;
		}
	}
	
	currentTurnDevice = nextDevice;
	setWaitingPlayer(nextDevice);

	for(var i=0; i<customHeadElements.length; i++) 
	{
	    setPlayerProfilePic(customHeadElements[i]);
	}
}

function turnStart()
{
	//vibrate(200);
	if (optionsOn == false || !dead)
	{
		activateMenu(MenuState.ACTION);
	}

	playButtonBack();
}

function turnEnd()
{
    if (!dead)
    {
        activateMenu(MenuState.WAITING);
    }
}

function setWaitingPlayer(waitingPlayerDevice)
{
	debugLog("WAITING PLAYER: " + waitingPlayerDevice.toString());
	var nickname = airconsole.getNickname(waitingPlayerDevice);
	if (nickname == undefined)
	{
		nickname = GetCachedName(waitingPlayerDevice);

		if (nickname == undefined)
		{
			nickname = "ENEMY";
		}
	}

	for (var i = 0; i < waitingPlayerTextElements.length; i++)
	{
		waitingPlayerTextElements[i].innerHTML = nickname.toUpperCase();
	}
}

function waitingOn()
{
	menuDivs[MenuState.WAITING].style.display = "block";
}

function loadLevelOn()
{
	menuDivs[MenuState.LOADLEVEL].style.display = "block";
}

function optionsOff()
{
	optionsOn = false;

	menuDivs[optionsReturnMenuState].style.display = 'block';
	menuDivs[MenuState.OPTIONS].style.display = 'none';

	currentMenuState = optionsReturnMenuState;
	debugLog("updated current menu state: " + currentMenuState.toString());

	if (currentTurnDevice != airconsole.getDeviceId() && 
		(optionsReturnMenuState == MenuState.ACTION))
	{
		waitingOn();
	}
}

function allowJoin()
{
	document.getElementById("joinButton").style.display = 'block';
	if (airconsole.getMasterControllerDeviceId() == airconsole.getDeviceId())
	{
		queryOptions();
		document.getElementById("joinHelpButton").style.display = 'block';
	}
	document.getElementById("loadingTitle").style.display = 'none';
	stopLoadingAnim();
}

function updateCharacterJoin(connectedIDs)
{
	//debugLog("updating connections: " + connectedIDs.length);
	
	var tempName;
	var numPlayersJoined = 0;
	for (var i = 0; i < connectedIDs.length; i++)
	{
		//debugLog(connectedIDs[i]);
		irisDevices[i] = connectedIDs[i];

		if (connectedIDs[i] == airconsole.getDeviceId())
		{
			setPlayerJoinVizThis(i);
			numPlayersJoined++;
		}
		else
		{
			tempName = airconsole.getNickname(connectedIDs[i]);
			if (tempName != undefined)
			{
				setPlayerJoinVizOther(i, connectedIDs[i]);
				numPlayersJoined++;
			}
			else
			{
				setPlayerJoinVizOpen(i);
			}
		}
	}

	if (numPlayersJoined == 0)
	{
		closeStartButton();
	}
}

function openStartButton()
{
	document.getElementById("readyStartButton").style.left = '40.5%';
}

function closeStartButton()
{
	document.getElementById("readyStartButton").style.left = '110%';
}

function openAdvanceSessionButton()
{
	document.getElementById("advanceSessionButton").style.left = '40.5%';
}

function closeAdvanceSessionButton()
{
	document.getElementById("advanceSessionButton").style.left = '110%';
}

function hideMovesRemaining()
{
	for (var i = 0; i < movesRemainingContainerElements.length; i++)
	{
		movesRemainingContainerElements[i].style.display = "none";
	}
}

function showMovesRemaining()
{
	for (var i = 0; i < movesRemainingContainerElements.length; i++)
	{
		movesRemainingContainerElements[i].style.display = "block";
	}
}

function updateStats(stats)
{
	originalStats = stats;

	var leadZero;
	for (var i = 0; i < stats.length; i++)
	{
		if (i < upgradeStatTextElements.length 
			&& upgradeStatTextElements[i] != null 
			&& upgradeStatTextElements[i] != undefined)
		{
			leadZero = "";

			if (stats[i] < 100)
			{
				leadZero += "0";
			}

			if (stats[i] < 10)
			{
				leadZero += "0";
			}

			upgradeStatTextElements[i].innerHTML = leadZero + stats[i].toString();
		}
	}
}

function clearUpgradeAllocation()
{
	for (var i = 0; i < NUM_STATS; i++)
	{
		currentUpgradeStats[i] = 0;
	}

	availableSkillPts = originalSkillPts;
	setSkillPtsText(originalSkillPts);
	updateStats(originalStats);
}

function addStatPt(statType)
{
	currentUpgradeStats[statType]++;
	availableSkillPts--;

	setSkillPtsText(availableSkillPts);

	var leadZero = "";
	var newStat = originalStats[statType] + currentUpgradeStats[statType];

	if (newStat < 100)
	{
		leadZero += "0";
	}

	if (newStat < 10)
	{
		leadZero += "0";
	}

	upgradeStatTextElements[statType].innerHTML = leadZero + newStat.toString();
}

function confirmUpgrades()
{
	//debugLog("sending confirmUpgrade message: ");

	var message = 
	{
		'action': 'ConfirmUpgrade',
		'playerIndex' : currentIrisIndex,
		'stats' : currentUpgradeStats
	};

	airconsole.message(0, message);

	//clearUpgradeAllocation();
}

function loadItemCache(itemInfo0, itemInfo1, itemInfo2, itemInfo3)
{
	debugLog("trying to load cache");
	debugLog(itemInfo0.length);
	debugLog(itemInfo1.length);
	debugLog(itemInfo2.length);
	debugLog(itemInfo3.length);
	debugLog("-----");
	itemCacheInfos[0] = itemInfo0;
	itemCacheInfos[1] = itemInfo1;
	itemCacheInfos[2] = itemInfo2;
	itemCacheInfos[3] = itemInfo3;

	for (var i = 0; i < itemCacheInfos.length; i++)
	{
		if (itemCacheInfos[i].length > 0)
		{
			fillItemContainer(i, itemCacheInfos[i]);
			showItemContainer(i);
		}
		else
		{
			hideItemContainer(i);
		}
	}

	activateMenu(MenuState.ITEMSELECTION);
}

function onItemCacheClose(turnPlayerDevice)
{
	debugLog("on item cache close");

	activateMenu(MenuState.ACTION);
	if (turnPlayerDevice != airconsole.getDeviceId())
	{
		setWaitingPlayer(turnPlayerDevice);
		
		if (dead == false)
		{
			waitingOn();
		}
	}

	for (var i = 0; i < 4; i++)
	{
		setItemSelectVizOpen(i)
		setReadyTriangleColor(playerItemReadyTriangleElements[i], -1);
		itemCompareOff(i);
	}

	setBackgroundColor(currentIrisIndex);
	initItemDevices();
}

function onGameOver(gameOverStats)
{
	for (var i = 0; i < NUM_GAME_OVER_STATS; i++)
	{
		if (i < gameOverStats.length)
		{
			setGameOverStat(i, gameOverStats[i]);
		}
	}

	activateMenu(MenuState.GAMEOVER);
	disconnectReturnMenuState = MenuState.GAMEOVER;
}

function onDisconnectActivateUI(playerIndex, disconnectName)
{
	debugLog("ON DISCONNECT ACTIVATE UI: " + disconnectName);

	document.getElementById("disconnectName").innerHTML = disconnectName.toString();

	if (airconsole.getDeviceId() == airconsole.getMasterControllerDeviceId())
	{
		setDisconnectUI(true);
		activateMenu(MenuState.DISCONNECT);
	}
	else
	{
		if (spectating == true)
		{
			setDisconnectUI(false);
			activateMenu(MenuState.DISCONNECT);
		}
	}
}

function onDisconnectDeactivateUI(playerIndex, turnPlayerDevice)
{
	debugLog("ON DISCONNECT DEACTIVATE UI");

	disconnectOff(turnPlayerDevice);
}

function setDisconnectUI(masterController)
{
	debugLog("disconnect On");

	if (masterController == true)
	{
		document.getElementById("disconnectMaster").style.display = 'block';
		document.getElementById("disconnectSlave").style.display = 'none';
	}
	else
	{
		document.getElementById("disconnectMaster").style.display = 'none';
		document.getElementById("disconnectSlave").style.display = 'block';
	}
}

function disconnectOff(turnPlayerDevice)
{
	if (disconnectReturnMenuState != -1)
	{
		menuDivs[disconnectReturnMenuState].style.display = 'block';
		currentMenuState = disconnectReturnMenuState;
		debugLog("updated current menu state: " + currentMenuState.toString());

		if (turnPlayerDevice != airconsole.getDeviceId())
		{
			setWaitingPlayer(turnPlayerDevice);
			
			if (dead == false && spectating == false)
			{
				waitingOn();
			}
		}

		disconnectReturnMenuState = -1;
	}
	menuDivs[MenuState.DISCONNECT].style.display = 'none';
}

function loadMainMenu()
{
	var message = 
	{
		'action': 'ReturnToMainMenu'
	};

	airconsole.message(0, message);
}

function debugLog(text)
{
	//debugText.innerHTML += (text + "<br>");
}