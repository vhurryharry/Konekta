### Environment examples:
# $RESOURCE_GROUP = "workcloudapps-prod"
# $WEBAPP = "appwca"
# $SLOT = "test"

$ALWAYS_ON = if($SLOT -eq "staging" -or $SLOT -eq "") {"true"} else {"false"}
$SLOT_PARAM = if($SLOT){"--slot", $SLOT}

az webapp config set --resource-group $RESOURCE_GROUP --name $WEBAPP $SLOT_PARAM `
    --web-sockets-enabled true `
    --use-32bit-worker-process true `
    --http20-enabled true `
    --always-on ($ALWAYS_ON)

az webapp restart --resource-group $RESOURCE_GROUP --name $WEBAPP $SLOT_PARAM