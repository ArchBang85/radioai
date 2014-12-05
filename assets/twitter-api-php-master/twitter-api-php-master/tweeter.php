<?php

/** Set access tokens here - see: https://dev.twitter.com/apps/ **/
$settings = array(
    'oauth_access_token' => "2886873339-ZbCn0es5JOjAhy3fDFTvgY51l6cgTwxFri0RS24",
    'oauth_access_token_secret' => "kPOalrlGSHaZUkCtd8q8Vy8OxtYazFmdBx0P5tdtCzNco",
    'consumer_key' => "nYDmS7uemCggroQM2lC1KoDqi", // API Key
    'consumer_secret' => "LygVkjznVF2qWMvIyH1NDjwiCBSSLL5wcRbAynM3kFluLbfFgu" // API Secret
);

/** Set redirect (on failure) URL here **/
$redirecturl = 'http://www.doubledashgames.com/radioai/';

// https://github.com/Deozaan/twitter-api-php
require_once('TwitterAPIExchange.php'); // make sure this url is correct

$url = 'https://api.twitter.com/1.1/statuses/update.json';
$requestMethod = 'POST';

$myStatus = html_entity_decode(htmlentities($_GET["status"], ENT_COMPAT, "UTF-8", false));
$myHash = htmlentities($_GET["hash"], ENT_COMPAT, "UTF-8", false);
$scaryStatus = $_GET["status"];

$postfields = array(
	'status' => $myStatus,
);

// make sure we have both a status and a hash
if (!empty($myStatus) && isset($myStatus) && !empty($myHash) && isset($myHash)) {
	// verify the hashes match	
	$newHash = base64_encode(hash_hmac('sha1', $myStatus, $settings['oauth_access_token_secret'], true));
	if ($newHash === $myHash) {
		// hashes match, so send the tweet!
		$twitter = new TwitterAPIExchange($settings);
		echo $twitter ->buildOAuth($url, $requestMethod)->setPostfields($postfields)->performRequest();
	} else {
	?>
	<link rel="icon" type="image/png" href="icon.png">
	Failed.
	<script type="text/javascript">

	<!--

	window.location = "<?php echo $redirecturl ?>"

	//-->

	</script>
	<?php
	}
}
	?>
	<link rel="icon" type="image/png" href="icon.png">
	<script type="text/javascript">

	<!--

	window.location = "<?php echo $redirecturl ?>"

	//-->

	</script>