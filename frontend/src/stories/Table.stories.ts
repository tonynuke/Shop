import Table from '../modules/common/components/table/Table.vue';
import TextCell from '../modules/common/components/table/TextCell.vue';

export default {
    title: 'Example/Table',
    component: Table,
    argTypes: {
        columns: Array,
        rows: Array,
        isEditable: Boolean,
    },
};

const Template = (args: any) => ({
    // Components used in your story `template` are defined in the `components` object
    components: { Table },
    // The story's `args` need to be mapped into the template through the `setup()` method
    setup() {
        return { args };
    },
    // And then the `args` are bound to your component with `v-bind="args"`
    template: '<Table v-bind="args" />',
});

export const ReadonlyTable = Template.bind({});
ReadonlyTable.args = {
    isEditable: false,
    columns: [
        { name: "brand", title: "Brand", type: "TextCell" },
        { name: "name", title: "Name", type: "TextCell" },
        { name: "price", title: "Price", type: "TextCell" },
    ],
    rows: [
        {
            brand: { text: "Adidas" },
            name: { text: "TShirt" },
            price: { text: "100" },
        },
    ],
};