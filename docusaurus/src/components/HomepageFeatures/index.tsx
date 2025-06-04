import type {ReactNode} from 'react';
import clsx from 'clsx';
import Heading from '@theme/Heading';
import styles from './styles.module.css';
import Link from '@docusaurus/Link';

type FeatureItem = {
  title: string;
  Logo: string;
  href: string;
  description: ReactNode;
  alignment?: string;
  key?: number;
};

const FeatureList: FeatureItem[] = [
  {
    title: 'Consultancy',
    Logo: 'consulting_logo',
    href: '/consultancy-services', 
    description: (
      <>
        Build new software, accelerate your digital transformation and optimize your IT infrastructure with our IT consultancy.
      </>
    ),
  },
  {
    title: 'Infrastructure Review',
    Logo: 'infrastructure_review_logo',
    href: '/infrastructure-review', 
    description: (
      <>
        Modern IT infrastructure is the backbone of any successful business. At Kadense Limited, our infrastructure review service is designed to help organizations optimize their technology environments for cost, resilience, scalability, and manageability.
      </>
    ),
  },
  {
    title: 'Open Source Software',
    Logo: 'oss_logo',
    href: '/open-source',
    description: (
      <>
        We at Kadense Limited are committed to the open source community, contributing to and supporting projects that drive innovation and foster collaboration. Our expertise in open source software allows us to help organisations leverage these technologies to build secure, scalable, and cost-effective solutions.
      </>
    )
  },
  {
    title: 'Software Development',
    Logo: 'software_development_logo',
    href: '/software-development',
    description: (
      <>
        Kadense Limited offers expert software development consultancy services, helping organisations deliver robust, scalable, and maintainable solutions tailored to their unique needs. Our team brings years of experience across a wide range of industries and project types, from greenfield applications to complex integrations and legacy system modernisation.
      </>
    ),
  },
  {
    title: 'Jupyternetes',
    Logo: 'jupyternetes_logo',
    href: '/docs/Products/Jupyternetes/Introduction', 
    description: (
      <>
        Jupyternetes enables running Jupyter Notebooks within a Kubernetes cluster, harnessing Kubernetes' power for data science and machine learning workflows.
      </>
    ),
  },
  {
    title: 'The Kadense Framework',
    Logo: 'kadense_logo',
    href: '/docs/intro', 
    description: (
      <>
        The Kadense Framework is an ecosystem for all your data needs in a secure environment. Whether you're a healthcare organization managing secure patient data or a bank processing account transactions, The Kadense Framework can help you build a secure, scalable, and cost-effective solution that runs on any infrastructure using an open-source foundation.
      </>
    ),
  }
];

function Feature(item: FeatureItem) {
  return (
    <div className={clsx('row', item.alignment)}>
      <FeatureImage alignClass="featureImageRight" {...item} />
      <FeatureContent alignClass="text--left" {...item} />
    </div>
  );
}

function FeatureImage({Logo} : FeatureItem) {
  return (
    <div className={clsx('col col--4')}>
      <div className={styles.feature}> 
        <div className={clsx('featureImage', Logo)}>
          &nbsp;
        </div>
      </div>
    </div>
  );
}



function FeatureContent({title, href, description} : FeatureItem) {
  return (
    <div className={clsx('col col--6 featureContent')}>
      <div className={styles.feature}> 
        <div className={clsx('padding-horiz--md')}>
          <Heading as="h3">{title}</Heading>
          <p>{description}</p>
        </div>
        <div className={clsx('padding-horiz--md featureLink')}>
          <Link className="button button--secondary button--lg" to={href}>
            Learn more
          </Link>
        </div>
      </div>
    </div>
  );
}

export default function HomepageFeatures(): ReactNode {
  return (
    <section className={styles.features}>
      <div className="container">
          {FeatureList.map((props, idx) =>
            <Feature alignment={idx % 2 === 0 ? "rowLTR" : "rowRTL"} key={idx} {...props} />
          )}
      </div>
    </section>
  );
}
