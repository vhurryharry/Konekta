import * as React from 'react'

import { TagPicker, ITag } from 'office-ui-fabric-react/lib/Pickers'

import { ErrorViewModel, SubscriberSearchResponseType } from 'utils/wcaApiTypes';
import { getRequest } from 'utils/request';

type AppProps = {
    searchBoxId: string,
    onChangeSubscriber: (newSubscriber: string | number) => void;
}

type AppStates = {
    dataLoaded: boolean,
    subscriberSearchResponse: SubscriberSearchResponseType | null;
    error: ErrorViewModel | null;
}

export default class SubscriberSearchBox extends React.Component<AppProps, AppStates> {
    constructor(props: AppProps) {
        super(props);

        this.state = {
            dataLoaded: false,
            subscriberSearchResponse: null,
            error: null
        }
    }

    render(): JSX.Element {
        return (
            <div>
                <TagPicker
                    onResolveSuggestions={this.onFilterChanged}
                    onChange={this.onChangeSubscriber}
                    getTextFromItem={this.getTextFromItem}
                    itemLimit={1}
                    pickerSuggestionsProps={{
                        suggestionsHeaderText: "Searched Subscribers",
                        noResultsFoundText: "No Subscriber Found"
                    }}
                    resolveDelay={300}
                />
            </div>
        )
    }

    private getTextFromItem = (item: ITag): string => {
        return item.name;
    }

    private onFilterChanged = async (filterText: string, selectedItems: ITag[] | undefined): Promise<ITag[]> => {

        if (filterText.length < 3) return [];

        let url = `/api/conveyancing/search-subscriber?subscriberName=${filterText}`;
        const response = (await getRequest(url)) as SubscriberSearchResponseType;

        let resultList: ITag[] = [];
        if (response.subscriber) {
            resultList = response.subscriber.map((subscriber, index) => {
                let name = `${subscriber.subscriberName!}`;
                if (subscriber.identification) {
                    name += `(${subscriber.identification.identifier!}:${subscriber.identification.value!})`;
                }

                return {
                    key: subscriber.subscriberId!,
                    name: name
                }
            })
        }

        return resultList;
    }

    private onChangeSubscriber = (items: ITag[] | undefined): void => {
        if (items && items.length > 0) {
            this.props.onChangeSubscriber(items[0].key);
        } else {
            this.props.onChangeSubscriber(0);
        }
    }
}